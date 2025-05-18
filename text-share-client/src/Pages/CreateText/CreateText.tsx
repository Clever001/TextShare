import React, { useEffect, useRef, useState } from 'react'
import * as monaco from 'monaco-editor';
import { getSyntaxes } from '../../Services/HelperService';
import './CreateText.css';
import Cookies from 'js-cookie';
import { CreateTextDto } from '../../Dtos';
import { CreateTextAPI } from '../../Services/API/TextAPIService';
import { isExceptionDto } from '../../Services/ErrorHandler';
import { useNavigate } from 'react-router-dom';


type Props = {}

const CreateText = (props: Props) => {
  const navigate = useNavigate();

  const editorRef = useRef<HTMLDivElement>(null);
  const editorInstance = useRef<monaco.editor.IStandaloneCodeEditor | null>(null);

  const [errors, setErrors] = useState<string[]>([]);

  const openAuth = () => { window.open(process.env.REACT_APP_CLIENT_URL_PATH + "auth/", "_blank"); }


  useEffect(() => {
    let isMounted = true;

    const token = Cookies.get("token");
    if (!token) {
      alert("Перед созданием текста вы должны сначала зарегистрироваться в системе.");
      navigate("/auth");
      return;
    }

    const initializeEditor = async () => {
      if (!isMounted) return;

      if (!editorRef.current) {
        for (let i = 0; i != 20; i++) {
          console.log("Жду", i);
          await new Promise(resolve => setTimeout(resolve, 100));
          if (editorRef.current) break;
        }
      }

      if (editorRef.current && !editorInstance.current) {
        try {
          editorInstance.current = monaco.editor.create(editorRef.current, {
            value: "",
            language: "plaintext",
            theme: 'vs',
          });
        } catch (error) {
          console.error("Ошибка при инициализации Monaco Editor:", error);
        }
      }
    }

    initializeEditor();

    const handleResize = () => {
      if (editorInstance.current) {
        editorInstance.current.layout();
      }
    };

    window.addEventListener('resize', handleResize);

    return () => {
      isMounted = false;
      if (editorInstance.current) {
        editorInstance.current.dispose();
        editorInstance.current = null;
      }
      window.removeEventListener('resize', handleResize);
    }

  }, []);

  const [hasPassword, setHasPassword] = useState<boolean>(false);
  const [highlightSyntax, setHighlightSyntax] = useState<boolean>(true);
  const [syntax, setSyntax] = useState<string>("plaintext");

  const updateHasPassword = (v:boolean) => {
    setHasPassword(v);
  }
  const updateHighlightSyntax = (v:boolean) => {
    if (!highlightSyntax && editorInstance.current) {
      const model = editorInstance.current.getModel();
      if (model) {
        monaco.editor.setModelLanguage(model, syntax);
      }
    } else if (editorInstance.current) {
      const model = editorInstance.current.getModel();
      if (model) {
        monaco.editor.setModelLanguage(model, "plaintext");
      }
    }
    setHighlightSyntax(v);
  }
  const updateSyntax = (v:string) => {
    setSyntax(v);
    if (highlightSyntax && editorInstance.current) {
      const model = editorInstance.current.getModel();
      if (model) {
        monaco.editor.setModelLanguage(model, v);
      }
    } else if (editorInstance.current) {
      const model = editorInstance.current.getModel();
      if (model) {
        monaco.editor.setModelLanguage(model, "plaintext");
      }
    }
  }

  const onFormSubmit = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();
    setErrors([]);

    const token = Cookies.get("token");
    if (!token) {
      alert("Перед созданием текста необходимо сначала зарегистрироваться на сайте.");
      openAuth();
      return;
    }

    const formData = new FormData(e.currentTarget);

    const getFormValue = (s: string): string => {
      var data = formData.get(s)?.toString() || "";
      return data;
    }

    if (!editorInstance.current) {
      setErrors(["Неудалось получить текст"]);
      return;
    }

    const createDto: CreateTextDto = {
      title: getFormValue("title"),
      description: getFormValue("description"),
      content: editorInstance.current?.getValue() ?? "",
      syntax: getFormValue("syntax"),
      tags: getFormValue("tags").split(" "),
      accessType: getFormValue("accessType"),
      password: hasPassword ? getFormValue("password") : null
    }

    const response = await CreateTextAPI(createDto, token);

    if (isExceptionDto(response)) {
      switch (response.httpCode) {
        case 401:
          alert("Вам нужно повторить авторизацию прежде чем создать текст.");
          openAuth();
          return;
        default:
          if (response.details) 
            setErrors(response.details);
          else
            setErrors([response.description]);
          return;
      }
    }

    navigate("/reader/" + response.id);
  }

  return (
    <div className="creation">
      <div className="text">
        {errors &&
          errors.map(e => {
            return (<div key={e} className="error">{e}</div>)
          })
        }
        <form onSubmit={onFormSubmit}>
          <div className="header">
            <div className="title">Новый текст</div>
            <div className="actions">
              <div className="switch-container">
                <p>Подсветка синтаксиса</p>
                <label className="switch">
                  <input type="checkbox" name="highlightSyntax" checked={highlightSyntax} onChange={e => updateHighlightSyntax(e.target.checked)}/>
                  <span className="slider"></span>
                </label>
              </div>
            </div>
          </div>
          <div className="content" ref={editorRef}></div>
          <div className="settings">
            <div className="title">Дополнительные настройки</div>
            <table>
              <tbody>
                <tr>
                  <td className="col1"><p>Заголовок текста</p></td>
                  <td className="col2"><input type="text" name="title" placeholder="Заголовок" /></td>
                </tr>
                <tr>
                  <td className="col1">
                    <p>Описание</p>
                  </td>
                  <td className="col2">
                    <textarea name="description" placeholder="Описание" />
                  </td>
                </tr>
                <tr>
                  <td className="col1"><p>Теги</p></td>
                  <td className="col2"><input type="text" name="tags" placeholder="Пример: 'тег1 тег2 тег3'" /></td>
                </tr>
                <tr>
                  <td className="col1"><p>Тип синтаксиса</p></td>
                  <td className="col2"><select name="syntax" value={syntax} onChange={e => updateSyntax(e.target.value)}>
                    {getSyntaxes().map(s => {
                      return (
                        <option key={s} value={s}>{s}</option>
                      );
                    })}
                  </select></td>
                </tr>
                <tr>
                  <td className="col1"><p>Тип доступа</p></td>
                  <td className="col2">
                    <select name="accessType" defaultValue="Personal">
                      <option value="ByReferencePublic">Публичный</option>
                      <option value="ByReferenceAuthorized">Публичный с авторизацией</option>
                      <option value="OnlyFriends">Только для друзей</option>
                      <option value="Personal">Приватный</option>
                    </select>
                  </td>
                </tr>
                <tr>
                  <td className="col-1"><p>Наличие пароля</p></td>
                  <td>
                    <input type="checkbox" name="hasPassword" checked={hasPassword} onChange={e => updateHasPassword(!hasPassword)} />
                  </td>
                </tr>
                <tr>
                  <td className="col1"><p>Пароль</p></td>
                  <td className="col2">
                    <input type="password" name="password" disabled={!hasPassword} placeholder="Пароль не может быть пустым" />
                  </td>
                </tr>
              </tbody>
            </table>
          </div>
          <button type="submit">Добавить новый текст</button>
        </form>
      </div>
    </div>
  )
}

export default CreateText