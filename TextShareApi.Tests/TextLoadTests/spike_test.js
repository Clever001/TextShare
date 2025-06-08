import http from "k6/http";
import { check, sleep } from 'k6';
import { UUID } from 'https://cdn.jsdelivr.net/npm/uuidjs@5.1.0/dist/uuid.min.js';

export let options = {
    insecureSkipTLSVerify: true,
    noConnectionReuse: false,
    stages: [
    { duration: '1m', target: 100 },
    { duration: '1m', target: 100 },
    { duration: '10s', target: 1000 },
    { duration: '3m', target: 1000 },
    { duration: '10s', target: 100 },
    { duration: '3m', target: 100 },
    { duration: '10s', target: 0 },
  ],
    thresholds: {
        checks: ['rate >= 0.9'], // Все чеки должны пройти успешно
        http_req_failed: ['rate < 0.05'] // Меньше 5% неудачных запросов
    }
};

const total_text_creation = 1000;
const file = open('../Texts/MediumText.txt', 'r');

function uuidv4() {
  return "10000000-1000-4000-8000-100000000000".replace(/[018]/g, c =>
    (+c ^ crypto.getRandomValues(new Uint8Array(1))[0] & 15 >> +c / 4).toString(16)
  );
}


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

    const ids = [];
    const curDate = new Date();
    curDate.setDate(curDate.getDate() + 1);
    const headers = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };
    for (let i = 0; i != total_text_creation; i++) {
        const response = http.post('https://localhost:7285/api/text', 
            JSON.stringify({
                title: uuidv4(),
                description: "",
                content: file,
                stynax: "plaintext",
                tags: [],
                accessType: "ByReferenceAuthorized",
                password: null,
                expiryDate: curDate
            }), {headers}
        )
        if (response.status !== 201) {
            console.error(`Failed to create text: ${response.status} - ${response.body}`);
            console.error(response.request.body.substring(0, 100));
            continue;
        }

        ids.push(response.json('id'));
    }

    return {token, ids};
}

export default (data) => {
    const token = data.token;
    const ids = data.ids;
    const id = ids[Math.floor(Math.random() * ids.length)]
    const headers = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };

    const response = http.get('https://localhost:7285/api/text/' + id, {headers} )

    check(response, {
        [`Text received successfully`]: (r) => r.status === 200 && r.json('content') !== ''
    });

    sleep(1);

    if (response.status !== 200) {
        fail(`Request for ID=${id} returned status ${response.status}`);
    }
};

export function teardown(data) {
    const token = data.token;
    const ids = data.ids;
    const headers = {
        'Content-Type': 'application/json',
        'Authorization': `Bearer ${token}`
    };
    for (let i = 0; i != ids.length; i++) {
        const response = http.request('DELETE', `https://localhost:7285/api/text/${ids[i]}`, null, {
            headers: headers
        });

        if (response.status !== 204) {
            console.error(`Failed to create text: ${response.status} - ${response.body}`);
            continue;
        }
    }
}