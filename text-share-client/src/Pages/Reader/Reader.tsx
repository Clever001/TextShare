import React, { useEffect, useRef, useState } from 'react';
import { Link, useNavigate, useParams } from 'react-router-dom';
import { DeleteTextAPI, SearchTextByIdAPI } from '../../Services/API/TextAPIService';
import { TextWithContentDto } from '../../Dtos';
import * as monaco from 'monaco-editor';
import './Reader.css';
import { isExceptionDto } from '../../Services/ErrorHandler';
import Cookies from 'js-cookie';

type Props = {};

const Reader = (props: Props) => {
  const editorRef = useRef<HTMLDivElement>(null);
  const editorInstance = useRef<monaco.editor.IStandaloneCodeEditor | null>(null);

  const { textId } = useParams();
  const [error, setError] = useState<string | null>(null);
  const [text, setText] = useState<TextWithContentDto | null>(null);
  const [password, setPassword] = useState<string>("");
  const [isPasswordRequired, setIsPasswordRequired] = useState<boolean>(false);

  const navigate = useNavigate();

  // Получение текста с сервера
  const getText = async (): Promise<TextWithContentDto | null> => {
    const token: string | null = Cookies.get('token') ?? null;

    if (typeof textId === "undefined") {
      setError('Текста с таким id нет!');
      setText(null);
      return null;
    }

    const result = await SearchTextByIdAPI(textId, token, password);

    if (isExceptionDto(result)) {
      switch (result.httpCode) {
        case 404: // Not Found
          setError("Данный текст не был найден или не существует.");
          break;
        case 403: // Forbidden
          setError("У вас недостаточно прав для доступа к данному тексту.");
          break;
        case 400: // Bad Request (не был предоставлен пароль)
          setIsPasswordRequired(true);
          setError("Заполните, пожалуйста, пароль для доступа к данному тексту.");
          break;
        default:
          setError(result.description);
          break;
      }
      setText(null);
      return null;
    }

    setError(null);
    setText(result);
    return result;
  };

  // Обработка ввода пароля
  const handlePasswordSubmit = async () => {
    const token: string | null = Cookies.get('token') ?? null;

    if (typeof textId === "undefined") {
      setError('Текста с таким id нет!');
      setText(null);
      return;
    }

    const result = await SearchTextByIdAPI(textId, token, password);

    if (isExceptionDto(result)) {
      if (result.httpCode === 400) {
        setError("Неверный пароль. Попробуйте снова.");
      } else {
        setError(result.description);
      }
      return;
    }

    setIsPasswordRequired(false);
    setError(null);
    setText(result);

    // Инициализируем редактор после успешной загрузки текста
    initializeEditor(result);
  };

  // Инициализация Monaco Editor
  const initializeEditor = async (loadedText: TextWithContentDto) => {
    if (!editorRef.current) {
      for (let i = 0; i != 20; i++) {
        console.log("Жду", i);
        await new Promise(resolve => setTimeout(resolve, 100));
        if (editorRef.current) break;
      }
    }

    console.log("!editorRef.current", !editorRef.current)
    console.log("!loadedText", !loadedText);

    if (!editorRef.current || !loadedText) return;

    if (editorInstance.current) {
      editorInstance.current.dispose(); // Уничтожаем предыдущий экземпляр
      editorInstance.current = null;
    }

    editorInstance.current = monaco.editor.create(editorRef.current, {
      value: loadedText.content,
      language: loadedText.syntax ? loadedText.syntax.toLowerCase() : "plaintext",
      theme: 'vs',
      readOnly: true,
    });
  };

  // Обновление содержимого редактора при изменении текста
  useEffect(() => {
    const updateText = async () => {
      if (!editorRef.current) {
        for (let i = 0; i != 20; i++) {
          console.log("Жду", i);
          await new Promise(resolve => setTimeout(resolve, 100));
          if (editorRef.current) break;
        }
      }

      if (text && editorInstance.current) {
        const model = editorInstance.current.getModel();
        if (model) {
          model.setValue(text.content);
          monaco.editor.setModelLanguage(model, text.syntax ? text.syntax.toLowerCase() : "plaintext");
        }
      }
    }

  }, [text]);

  // Загрузка текста и инициализация редактора
  useEffect(() => {
    let isMounted = true;
    var pwdRequired = isPasswordRequired;

    const loadTextAndInitializeEditor = async () => {
      const loadedText = await getText();
      if (!loadedText || !isMounted) return;

      if (!pwdRequired) {
        initializeEditor(loadedText);
      }
    };

    setIsPasswordRequired(false); // Сбрасываем флаг при изменении textId
    pwdRequired = false;
    loadTextAndInitializeEditor();

    return () => {
      isMounted = false;
      editorInstance.current?.dispose(); // Уничтожаем экземпляр при размонтировании
      editorInstance.current = null;
    };
  }, [textId]);

  // Функция для преобразования даты
  const convertDate = (d: Date): string => {
    return `${d.getDate()}.${d.getMonth() + 1}.${d.getFullYear()}`;
  };

  const convertDateTime = (d: Date): string => {
    const hours = String(d.getHours()).padStart(2, '0');
    const minutes = String(d.getMinutes()).padStart(2, '0');

    return `${d.getDate()}.${d.getMonth() + 1}.${d.getFullYear()} ${hours}:${minutes}`;
  };

  // Логика копирования текста
  const [isCopied, setIsCopied] = useState<boolean>(false);
  const onCopy = async (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
    if (text) {
      try {
        await navigator.clipboard.writeText(text.content);
        setIsCopied(true);

        setTimeout(() => {
          setIsCopied(false);
        }, 3000);
      } catch (error) {
        console.error("Ошибка при копировании текста:", error);
      }
    }
  };

  const handleDownload = async (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
    if (text) {
      try {
        const types = monaco.languages.getLanguages().find(l => l.id === text.syntax)?.extensions;
        const extension = types ? types[0] : ".txt";

        const blob = new Blob([text.content], { type: "text/plain" });
        const url = URL.createObjectURL(blob);

        const link = document.createElement('a');
        link.href = url;
        link.download = text.title + extension;

        document.body.appendChild(link);

        link.click();

        document.body.removeChild(link);
        URL.revokeObjectURL(url);
      } catch (error) {
        console.log(error);
        alert("Произошла ошибка при закачивании текста.");
      }
    }
  }

  const deleteText = async (e: React.MouseEvent<HTMLAnchorElement, MouseEvent>) => {
    if (!textId) {
      setError("Не был задан id для удаляемого текста.");
      return;
    }

    const token = Cookies.get("token");
    if (!token) {
      alert("Перед удалением текста вам нужно сначала зарегистрироваться в системе.");
      navigate("/auth");
      return;
    }

    const result = await DeleteTextAPI(textId, token);

    if (isExceptionDto(result)) {
      setError(result.description);
      return;
    }

    alert("Текст был успешно удален.");
    window.location.reload();
  }

  return (
    <div className="reader">
      {/* Форма ввода пароля */}
      {isPasswordRequired && (
        <div className="password-prompt">
          <p>Введите пароль для доступа к тексту:</p>
          <input
            type="password"
            value={password}
            onChange={(e) => setPassword(e.target.value)}
            placeholder="Пароль"
          />
          <button onClick={handlePasswordSubmit}>Подтвердить</button>
        </div>
      )}

      {error &&
        <div className="error">{error}</div>
      }

      {/* Отображение текста */}
      {text && (
        <div className="text">
          <div className="header">
            <div className="info">
              <img src="img/user_icon_black.svg" alt="user" />
              <div className="text-type-info">
                <p className="title">{text.title}</p>
                <div className="sub-info">
                  <Link to={`/profile/${encodeURIComponent(text.ownerName)}`}>
                    <p className="owner-name">{text.ownerName}</p>
                  </Link>
                  <p className="date">{convertDate(text.createdOn)}</p>
                  <p className="syntax">{text.syntax}</p>
                  <p className="expiryDate">время удаления: {convertDateTime(text.expiryDate)}</p>
                </div>
              </div>
            </div>
            <div className="actions">
              <a onClick={onCopy} className="copy-button">
                <img src="img/copy_black.svg" alt="copy" />
                {isCopied && (
                  <span className="copy-notification">Текст скопирован!</span>
                )}
              </a>
              <Link to={`/editor/${text.id}`}><img src="img/edit_black.svg" alt="edit" /></Link>
              <a onClick={handleDownload}><img src="img/download_black.svg" alt="download" /></a>
              <a onClick={deleteText}><img src="img/delete_black.svg" alt="delete" /></a>
            </div>
          </div>
          <div className="content" ref={editorRef} style={{ height: '80vh' }} />
        </div>
      )}
    </div>
  );
};

export default Reader;