import { type SubmitEvent, type ChangeEvent, useContext, useState, useCallback } from 'react';
import ClientFormCard from '../../4-Widgets/ClientFormWidget/ClientFormWidget';
import './ClientFormFeature.css';
import { UserContext } from '../../1-Processes/UserContext';
import { useNavigate } from 'react-router-dom';
import { ROUTES } from '../../Router/Router';

export default function ClientFormFeature() {
  const userContext = useContext(UserContext);
  const [userName, setUserName] = useState<string>(() => {
    const user = userContext.getUser();
    return user ? user.name : "";
  });
  const navigate = useNavigate();

  const onUserNameChange = useCallback((event: ChangeEvent<HTMLInputElement, HTMLInputElement>) => {
    event.preventDefault();
    const newName = event.target.value;
    setUserName(newName);
  }, []);
  const onSubmit = useCallback((event: SubmitEvent<HTMLFormElement>) => {
    event.preventDefault();
    if (userName.trim() === "") {
      return;
    }

    userContext.setUser(userName);
    navigate(ROUTES.DOCUMENT);
  }, [userContext, userName]);
  

  return (  
    <div className="client-form-container">
      <ClientFormCard onSubmit={onSubmit} userName={userName} onUserNameChange={onUserNameChange}/>
    </div>
  )
}