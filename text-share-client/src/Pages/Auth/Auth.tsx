import React, { useContext, useState } from 'react';
import './Auth.css';
import { LoginAPI, RegisterAPI } from '../Services/AuthService';
import { UserWithTokenDto } from '../../Dtos';
import Cookies from 'js-cookie';
import { useNavigate } from 'react-router';
import { AuthContext } from '../../Context/AuthContext';

type Props = {}

const Auth = ({}: Props) => {
  const [showLogin, setShowLogin] = useState<boolean>(true); // else show Register form
  const [errors, setErrors] = useState<string[]>([]);
  const navigate = useNavigate();


  const authContext = useContext(AuthContext);
  if (!authContext) {
    throw new Error('AuthContext must be used within a AuthProvider');
  }
  // const validAuth = authContext.validAuth;
  const setValidAuth = authContext.setValidAuth;

  const isValidForm = (f: HTMLFormElement):boolean => {
    var isValid:boolean = true;
    var errorMessages:string[] = [];
    if (showLogin) {
      if ((f[0] as HTMLInputElement).value === "") {
        errorMessages.push("Имя пользователя не может быть пустым!");
        isValid = false;
      }
      if ((f[1] as HTMLInputElement).value === "") {
        errorMessages.push("Пароль не может быть пустым!");
        isValid = false;
      }
    } else {
      if ((f[0] as HTMLInputElement).value === "") {
        errorMessages.push("Имя пользователя не может быть пустым!");
        isValid = false;
      }
      if ((f[1] as HTMLInputElement).value === "") {
        errorMessages.push("Электронная почта не может быть пустым!");
        isValid = false;
      }
      if ((f[2] as HTMLInputElement).value === "") {
        errorMessages.push("Пароль не может быть пустым!");
        isValid = false;
      }
      if ((f[3] as HTMLInputElement).value === "") {
        errorMessages.push("Введите пароль второй раз!");
        isValid = false;
      } else if ((f[3] as HTMLInputElement).value !== (f[2] as HTMLInputElement).value) {
        errorMessages.push("Пароли не совпадают!");
        isValid = false;
      }
    }
  
    setErrors(errorMessages)
    return isValid;
  }

  const onLoginSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const form = e.currentTarget;
    if (!isValidForm(form)) {
      return;
    }

    const userNameOrEmail = (form[0] as HTMLInputElement).value;
    const password = (form[1] as HTMLInputElement).value;

    const result = await LoginAPI({
      userNameOrEmail:userNameOrEmail, 
      password:password
    });
    
    if (Array.isArray(result)) {
      setErrors(result);
      return;
    }

    const user = result.data as UserWithTokenDto;
    console.log(user);
    console.log(user.userName);
    Cookies.set("userName", user.userName, {expires:1});
    Cookies.set("email", user.email, {expires:1});
    Cookies.set("token", user.token, {expires:1});
    setValidAuth(true);
    navigate("/");
  }

  const onChangeButtonClick = (e: React.MouseEvent<HTMLButtonElement, MouseEvent>) => {
    const curVal = showLogin;
    setShowLogin(!curVal);
    setErrors([]);
  }

  const onRegisterSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const form = e.currentTarget;
    if (!isValidForm(form)) {
      return;
    }

    const userName = (form[0] as HTMLInputElement).value;
    const email = (form[1] as HTMLInputElement).value;
    const password = (form[2] as HTMLInputElement).value;

    const result = await RegisterAPI({
      userName:userName,
      email:email,
      password:password
    });

    if (Array.isArray(result)) {
      setErrors(result);
      return;
    }

    var user = result.data as UserWithTokenDto;
    console.log(user);
    Cookies.set("userName", user.userName, {expires:1});
    Cookies.set("email", user.email, {expires:1});
    Cookies.set("token", user.token, {expires:1});
    setValidAuth(true);
    navigate("/");
  }

  return (
    <div className="auth">
      {showLogin ? 
      <div className="login">
        <div className="title">Вход в учетную запись</div>
        <form onSubmit={onLoginSubmit}>
          <div>
            <label htmlFor="userName">Имя пользователя или почта</label>
            <input type="text" id="userName"/>
          </div>
          <div>
            <label htmlFor="password">Пароль</label>
            <input type="text" id="userName" />
          </div>
          <div>
            {errors.length > 0 && 
              <div className="error-messages">
                {errors.map((e) => (
                  <p key={e}>{e}</p>
                ))}
              </div>
            }
            <button type="submit">Войти</button>
            <p>Нет учетной записи?</p>
            <button type="button" onClick={onChangeButtonClick}>Зарегистрироваться</button>
          </div>
        </form>
      </div>
      :
      <div className="register">
        <div className="title">Регистрация</div>
        <form onSubmit={onRegisterSubmit}>
          <div>
            <label htmlFor="userName">Имя пользователя</label>
            <input type="text" id="userName"/>
          </div>
          <div>
            <label htmlFor="email">Электронная почта</label>
            <input type="text" id="email" />
          </div>
          <div>
            <label htmlFor="password">Пароль</label>
            <input type="text" id="password" />
          </div>
          <div>
            <label htmlFor="second-password">Повторите пароль</label>
            <input type="text" id="second-password" />
          </div>
          <div>
            {errors.length > 0 && 
              <div className="error-messages">
                {errors.map((e, i) => (
                  <p key={i}>{e}</p>
                ))}
              </div>
            }
            <button type="submit">Зарегистрироваться</button>
            <p>Уже есть учетная запись?</p>
            <button type="button" onClick={onChangeButtonClick}>Войти</button>
          </div>
        </form>
      </div>
      }
    </div>
  )
}

export default Auth