import Cookies from 'js-cookie';
import React, { useEffect, useState } from 'react'
import { useNavigate } from 'react-router-dom';
import './ProfileContentChange.css';
import { UpdateUserDto } from '../../Dtos';
import { ChangeProfileContentAPI } from '../../Services/API/AccountAPIService';
import { isExceptionDto } from '../../Services/ErrorHandler';

type Props = {}

const ProfileContentChange = (props: Props) => {
  const navigate = useNavigate();

  const [userName, setUserName] = useState<string>("");
  const [email, setEmail] = useState<string>("");
  const [errors, setErrors] = useState<string[]>([]);

  useEffect(() => {
    const _userName = Cookies.get("userName");
    const _email = Cookies.get("email");
    if (!_userName || !_email) {
      alert("Перед редактированием профиля сначала необходимо зарегистрироваться в системе.");
      navigate("/auth");
      return;
    }

    setUserName(_userName);
    setEmail(_email);
  }, []);

  const onFormSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    setErrors([]);
    const token = Cookies.get("token");
    if (!token) {
      alert("Перед редактированием профиля сначала необходимо зарегистрироваться в системе.");
      navigate("/auth");
      return;
    }
    const formData = new FormData(e.currentTarget);

    const dto: UpdateUserDto = {
      userName: formData.get("userName")?.toString() ?? null,
      email: formData.get("email")?.toString() ?? null
    }

    const response = await ChangeProfileContentAPI(dto, token);
    if (isExceptionDto(response)) {
      setErrors(response.details ?? [response.description]);
      return;
    }

    Cookies.remove("id");
    Cookies.remove("token");
    Cookies.remove("email");
    Cookies.remove("userName");
    
    Cookies.set("id", response.id);
    Cookies.set("token", response.token);
    Cookies.set("email", response.email);
    Cookies.set("userName", response.userName);
    alert("Данные успешно изменены.");
    window.location.reload();
  }

  return (
    <div className="profile-change">
      {errors && errors.map(e => {
        return (
          <div className="error">{e}</div>
        )
      })}
      <div className="title">Мой профиль</div>
      <form onSubmit={onFormSubmit}>
        <table>
          <tbody>
            <tr>
              <td>
                <p>Имя пользователя</p>
                <input type="text" name="userName" defaultValue={userName} />
              </td>
              <td>
                <p>Электронная почта</p>
                <input type="text" name="email" defaultValue={email} />
              </td>
            </tr>
          </tbody>
        </table>
        <div className="mycenter">
          <button type="submit">Обновить данные</button>
        </div>
      </form>
    </div>
  )
}

export default ProfileContentChange