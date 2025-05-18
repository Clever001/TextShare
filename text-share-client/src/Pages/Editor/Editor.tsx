import React, { SyntheticEvent, useEffect, useRef, useState } from 'react'
import { TextWithContentDto, UpdateTextDto } from '../../Dtos'
import { useNavigate, useParams } from 'react-router';
import './Editor.css';
import Cookies from 'js-cookie';
import { SearchTextByIdAPI, UpdateTextAPI } from '../../Services/API/TextAPIService';
import { isExceptionDto } from '../../Services/ErrorHandler';
import * as monaco from 'monaco-editor';
import { getSyntaxes } from '../../Services/HelperService';

type Props = {}

const Editor = (props: Props) => {
  const navigate = useNavigate();

  const editorRef = useRef<HTMLDivElement>(null);
  const editorInstance = useRef<monaco.editor.IStandaloneCodeEditor | null>(null);

  const { textId } = useParams();
  const [errors, setErrors] = useState<string[]>([]);
  const [text, setText] = useState<TextWithContentDto | null>(null);

  const openAuth = () => { window.open(process.env.REACT_APP_CLIENT_URL_PATH + "auth/", "_blank"); }

  // Get Text From API

  var performingGetText: boolean = false;

  const getText = async (): Promise<TextWithContentDto | null> => {
    const token: string | null = Cookies.get('token') ?? null;

    if (!token) {
      alert("Перед изменением данного блока текста необходимо сначала зарегистрироваться в системе.");
      navigate("/auth");
      return null;
    }

    if (typeof (textId) === "undefined") {
      setErrors(['Текста c таким id нет!']);
      setText(null);
      return null;
    }

    const result = await SearchTextByIdAPI(textId, token, null);

    if (isExceptionDto(result)) {
      switch (result.httpCode) {
        case 404: // Not Found
          setErrors(["Данный текст не был найден или не существует."]);
          break;
        case 403: // Forbidden
          setErrors(["У вас недостаточно прав для доступа на редактирование данного текста."]);
          break;
        case 400: // Bad Request (не был предоставлен пароль)
          setErrors(["У вас недостаточно прав для доступа на редактирование данного текста."]);
          break;
        default:
          console.log("http code:", result.httpCode);
          setErrors([result.description]);
          break;
      }
      setText(null);
      return null;
    }

    const curUserName = Cookies.get("userName");
    if (!curUserName) {
      setErrors(["Информация о имени текущего пользователя не была найдена."]);
      return null;
    }
    if (curUserName != result.ownerName) {
      setErrors(["Изменять блоки текста может только владелец текста."]);
      return null;
    }

    result.tags.sort();

    setErrors([]);
    setText(result);

    setTitle(result.title);
    setDescription(result.description);
    setSyntax(result.syntax);
    setTags(result.tags.join(" "));
    setAccessType(result.accessType);
    setHasPassword(result.hasPassword);
    return result;
  }

  useEffect(() => {
    let isMounted = true; // Флаг для отслеживания состояния монтирования

    const initializeEditor = async () => {
      if (!isMounted) return; // Проверяем, монтирован ли компонент

      const text = await getText();
      if (!text || !isMounted) {
        return; // Выходим, если текст не загружен или компонент размонтирован
      }

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
            value: text.content,
            language: text.syntax ? text.syntax.toLowerCase() : "plaintext",
            theme: 'vs',
          });
        } catch (error) {
          console.error("Ошибка при инициализации Monaco Editor:", error);
        }
      }
    };

    initializeEditor();

    const handleResize = () => {
      if (editorInstance.current) {
        editorInstance.current.layout(); // Пересчитываем размеры редактора
      }
    };

    window.addEventListener('resize', handleResize);

    return () => {
      isMounted = false; // Сбрасываем флаг при размонтировании
      if (editorInstance.current) {
        editorInstance.current.dispose(); // Уничтожаем экземпляр редактора
        editorInstance.current = null;
      }
      window.removeEventListener('resize', handleResize);
    };
  }, [textId]);

  // Update Text By API

  const updateText = async (e: React.FormEvent<HTMLFormElement>) => {
    e.preventDefault();

    const token = Cookies.get("token");
    if (!token) {
      alert("Обновите свои регистрационные данные.");
      openAuth();
      return;
    }

    if (!text) {
      setErrors([...errors, "Нельзя изменить текст. Текст для изменения еще не был получен."]);
      return;
    }

    var updateDto: UpdateTextDto = {
      title: null,
      description: null,
      content: null,
      syntax: null,
      tags: null,
      accessType: null,
      password: null,
      updatePassword: false
    };



    // Title
    if (title != text?.title) { updateDto.title = title; }
    // Description
    if (description != text?.description) { updateDto.description = description; }
    // Content
    if (editorInstance.current?.getValue() != text?.content) { updateDto.content = editorInstance.current?.getValue() ?? null; }
    // Syntax
    if (syntax != text?.syntax) { updateDto.syntax = syntax; }
    // Tags
    const newTags = tags.split(" ");
    newTags.sort();
    const oldTags = text?.tags ?? [];
    if (newTags.length === oldTags.length) {
      var areEqual: boolean = true;
      for (let i = 0; i != oldTags.length; i++) {
        if (newTags[i] !== oldTags[i]) {
          areEqual = false;
          break;
        }
      }

      if (!areEqual) updateDto.tags = newTags;
    }
    // AccessType
    if (accessType != text?.accessType) { updateDto.accessType = accessType; }
    // Password
    if (!text?.hasPassword && hasPassword) {
      updateDto.updatePassword = true;
      updateDto.password = (password == "") ? null : password;
    } else if (text?.hasPassword) {
      if (!hasPassword) {
        updateDto.updatePassword = true;
        updateDto.password = null;
      } else if (updatePassword) {
        updateDto.updatePassword = true;
        updateDto.password = (password == "") ? null : password;
      }
    }

    if (updateDto.title == null && updateDto.accessType == null &&
      updateDto.content == null && updateDto.syntax == null &&
      updateDto.tags == null && updateDto.accessType == null &&
      updateDto.updatePassword == false && updateDto.description == null
    ) {
      setErrors(["Вы не внесли никаких изменений в текст."]);
      return;
    }

    const result = await UpdateTextAPI(text?.id, updateDto, token);

    if (isExceptionDto(result)) {
      switch (result.httpCode) {
        case 400: // BadRequest
          if (result.details) 
            setErrors(result.details);
          else
            setErrors([result.description]);
          break;
        case 401: // UnAuthorized
          alert("Обновите свои регистрационные данные перед изменением текста.");
          openAuth();
          break;
        case 403: // Forbidden
          alert("Вы не можете изменить этот текст, так как не являетесь его владельцем.");
          break;
        case 404: // NotFound
          setErrors(["Изменяемый текст не был найден в системе."]);
          break;
        default:
          setErrors([result.description]);
          break;
      }
      return;
    }

    const url = process.env.REACT_APP_CLIENT_URL_PATH + "reader/" + textId;

    window.open(url, "_self");
  }

  // Impl of text params modification
  const [title, setTitle] = useState<string>("");
  const [description, setDescription] = useState<string>("");
  const [syntax, setSyntax] = useState<string>("");
  const [tags, setTags] = useState<string>("");
  const [accessType, setAccessType] = useState<string>("");
  const [password, setPassword] = useState<string>("");
  const [hasPassword, setHasPassword] = useState<boolean>(false);
  const [updatePassword, setUpdatePassword] = useState<boolean>(false);

  const onTitleChange = (e: React.ChangeEvent<HTMLInputElement>) => { setTitle(e.target.value); }
  const onDescriptionChange = (e: React.ChangeEvent<HTMLTextAreaElement>) => { setDescription(e.target.value); }
  const onSyntaxChange = (e: React.ChangeEvent<HTMLSelectElement>) => {
    setSyntax(e.target.value);
    if (highlight && editorInstance.current) {
      const model = editorInstance.current.getModel();
      if (model) {
        monaco.editor.setModelLanguage(model, (e.target.value != "") ? e.target.value : "plaintext");
      }
    }
  }
  const onTagsChange = (e: React.ChangeEvent<HTMLInputElement>) => { setTags(e.target.value); }
  const onAccessTypeChange = (e: React.ChangeEvent<HTMLSelectElement>) => { setAccessType(e.target.value); }
  const onPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => { setPassword(e.target.value); }
  const onHasPasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    setHasPassword(e.target.checked);
    if (!e.target.checked) {
      setUpdatePassword(false)
      setPassword("");
    }
  }
  const onUpdatePasswordChange = (e: React.ChangeEvent<HTMLInputElement>) => { setUpdatePassword(e.target.checked); }

  const returnTitle = (e: SyntheticEvent) => { setTitle(text?.title ?? ""); }
  const returnDescription = (e: SyntheticEvent) => { setDescription(text?.description ?? ""); }
  const returnSyntax = (e: SyntheticEvent) => {
    setSyntax(text?.syntax ?? "plaintext");
    if (highlight && editorInstance.current) {
      const model = editorInstance.current.getModel();
      if (model) {
        monaco.editor.setModelLanguage(model, text?.syntax ?? "plaintext");
      }
    }
  }
  const returnTags = (e: SyntheticEvent) => { setTags(text?.tags?.join(" ") ?? ""); };
  const returnAccessType = (e: SyntheticEvent) => { setAccessType(text?.accessType ?? "") };
  const returnHasPassword = (e: SyntheticEvent) => { setHasPassword(text?.hasPassword ?? false); }

  // Text Highlight
  const [highlight, setHighlight] = useState<boolean>(true);

  const onHighlightChange = (e: React.ChangeEvent<HTMLInputElement>) => {
    if (!highlight && editorInstance.current) {
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
    setHighlight(!highlight);
  }

  return (
    <div className="editor">
      {text ?
        <div className="text">
          <form onSubmit={updateText}>
            <div className="header">
              <div className="title">Редактор текста</div>
              <div className="actions">
                <div className="switch-container">
                  <p>Подсветка синтаксиса</p>
                  <label className="switch">
                    <input type="checkbox" name="highlightSyntax" checked={highlight} onChange={onHighlightChange} />
                    <span className="slider"></span>
                  </label>
                </div>
              </div>
            </div>
            <div className="content" ref={editorRef} />
            <div className="settings">
              <div className="title">Дополнительные настройки</div>
              <table>
                <tbody>
                  <tr>
                    <td className="col1"><p>Заголовок текста</p></td>
                    <td className="col2"><input type="text" name="title" value={title} onChange={onTitleChange} /></td>
                    <td className="col3"><img src="img/return_black.svg" alt="return" onClick={returnTitle} /></td>
                  </tr>
                  <tr>
                    <td className="col1">
                      <p>Описание</p>
                    </td>
                    <td className="col2">
                      <textarea name="description" value={description} onChange={onDescriptionChange} />
                    </td>
                    <td className="col3"><img src="img/return_black.svg" alt="return" onClick={returnDescription} /></td>
                  </tr>
                  <tr>
                    <td className="col1"><p>Теги</p></td>
                    <td className="col2"><input type="text" name="tags" value={tags} onChange={onTagsChange} /></td>
                    <td className="col3"><img src="img/return_black.svg" alt="return" onClick={returnTags} /></td>
                  </tr>
                  <tr>
                    <td className="col1"><p>Тип синтаксиса</p></td>
                    <td className="col2"><select name="syntax" defaultValue="plaintext" value={syntax} onChange={onSyntaxChange}>
                      {/* <option value=""></option> */}
                      {getSyntaxes().map(s => {
                        return (
                          <option key={s} value={s}>{s}</option>
                        );
                      })}
                    </select></td>
                    <td className="col3"><img src="img/return_black.svg" alt="return" onClick={returnSyntax} /></td>
                  </tr>
                  <tr>
                    <td className="col1"><p>Тип доступа</p></td>
                    <td className="col2">
                      <select name="accessType" value={accessType} onChange={onAccessTypeChange}>
                        <option value=""></option>
                        <option value="ByReferencePublic">Публичный</option>
                        <option value="ByReferenceAuthorized">Публичный с авторизацией</option>
                        <option value="OnlyFriends">Только для друзей</option>
                        <option value="Personal">Приватный</option>
                      </select>
                    </td>
                    <td className="col3"><img src="img/return_black.svg" alt="return" onClick={returnAccessType} /></td>
                  </tr>
                  <tr>
                    <td className="col-1"><p>Наличие пароля</p></td>
                    <td>
                      <input type="checkbox" name="hasPassword" checked={hasPassword} onChange={onHasPasswordChange} />
                    </td>
                    <td className="col3"><img src="img/return_black.svg" alt="return" onClick={returnHasPassword} /></td>
                  </tr>
                  {text.hasPassword &&
                    <tr>
                      <td>
                        <p>Обновить пароль</p>
                      </td>
                      <td><input type="checkbox" checked={updatePassword} disabled={!hasPassword} onChange={onUpdatePasswordChange} /></td>
                    </tr>
                  }
                  <tr>
                    <td className="col1"><p>Пароль</p></td>
                    <td className="col2">
                      <input type="password" name="password" value={password} disabled={!hasPassword || text.hasPassword && !updatePassword} onChange={onPasswordChange} />
                    </td>
                  </tr>
                </tbody>
              </table>
            </div>
            {errors &&
              errors.map(e => {
                return (<div key={e} className="error">{e}</div>)
              })
            }
            <button type="submit">Изменить текст</button>
          </form>
        </div>
        :
        <div className="error">{errors}</div>
      }
    </div>
  )
}

export default Editor