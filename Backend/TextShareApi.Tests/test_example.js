import http from "k6/http";
import { check, sleep } from 'k6';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
        { duration: '1m', target: 300 },
        { duration: '3m', target: 300 },
        { duration: '1m', target: 0 },
    ]
};


// export default () => {
//     const res = http.get('https://localhost:7285/api/text/latests');
//     check(res, {
//         'is status 200': (r) => r.status === 200,
//     })
//     sleep(1);
// };

const public_text_id = 'CgAAAAAAAAA';
const public_auth_text_id = 'AgAAAAAAAAA';
const friends_text_id = 'CwAAAAAAAAA';
const private_text_id = 'BQAAAAAAAAA';

export function setup() {
    const tokenResponse = http.post('https://localhost:7285/api/accounts/login', 
        JSON.stringify({
            userNameOrEmail: __ENV.USERNAME,
            password: __ENV.PASSWORD
        }), {
            headers: { 'Content-Type': 'application/json' }
        });

    check(tokenResponse, {
        'Login successful': (res) => res.status === 200,
        'Token received': (res) => res.json('token') !== ''
    });

    if (tokenResponse.status !== 200 || !tokenResponse.json('token')) {
        console.error('Failed to get token. Response:', tokenResponse.body);
        console.error('Request: ', tokenResponse.request.body);
        throw new Error("Token not consumed.");
    }


    const token = tokenResponse.json('token');

    return {token};
}

export default (data) => {
    const token = data.token;
    const headers = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };

    // Создаем массив запросов для http.batch
    const requests = [
        { method: 'GET', url: `https://localhost:7285/api/text/${public_text_id}`,  params: { headers } },
        { method: 'GET', url: `https://localhost:7285/api/text/${public_auth_text_id}`,  params: { headers } },
        { method: 'GET', url: `https://localhost:7285/api/text/${friends_text_id}`,  params: { headers } },
        { method: 'GET', url: `https://localhost:7285/api/text/${private_text_id}`,  params: { headers } }
    ];

    // Отправляем все запросы одновременно
    const responses = http.batch(requests);

    // Проверяем каждый ответ
    responses.forEach((response, index) => {
        check(response, {
            [`Text ${index + 1} received successfully`]: (r) => r.status === 200 && r.json('content') !== ''
        });
    });

    sleep(1);
};