import Cookies from 'js-cookie';
import React, { useEffect, useRef, useState } from 'react'
import { Link, useParams } from 'react-router-dom';
import { SearchTextByIdAPI } from '../../Services/API/TextAPIService';
import { TextWithContentDto } from '../../Dtos';
import * as monaco from 'monaco-editor';
import './Reader.css'
import { isExceptionDto } from '../../Services/ErrorHandler';

type Props = {};

const Reader = (props: Props) => {
  const editorRef = useRef<HTMLDivElement>(null);
  const editorInstance = useRef<monaco.editor.IStandaloneCodeEditor | null>(null);

  const { textId } = useParams();
  const [error, setError] = useState<string | null>(null);
  const [text, setText] = useState<TextWithContentDto | null>(null);
  

  const getText = async (): Promise<TextWithContentDto | null> => {
    const token: string | null = Cookies.get('token') ?? null;

    if (typeof (textId) === "undefined") {
      setError('Текста c таким id нет!');
      setText(null);
      return null;
    }

    const result = await SearchTextByIdAPI(textId, token, null);

    if (isExceptionDto(result)) {
      switch (result.httpCode) {
        case 404: // Not Found
          setError("Данный текст не был найден или не существует.");
          break;
        case 403: // Forbidden
          setError("У вас недостаточно прав для доступа к данному тексту.");
          break;
        case 400: // Bad Request (не был предоставлен пароль)
          setError("Заполните, пожалуйста, пароль для доступа к данному тексту.");
          break;
        default:
          console.log("http code:", result.httpCode);
          setError(result.description);
          break;
        }
      setText(null);
      return null;
    }

    setError(null);
    setText(result);
    return result;
  }

  useEffect(() => {
    let isMounted = true;

    const initializeEditor = async () => {
      const text = await getText();
      if (!text || text.content == "" || !isMounted) {
        return;
      }

      if (!editorRef.current) {
        for (let i = 0; i != 20; i++) {
          console.log("Жду", i);
          await new Promise( resolve => setTimeout(resolve, 100) );
          if (editorRef.current) break;
        }
      }

      if (editorRef.current && !editorInstance.current) {
        editorInstance.current = monaco.editor.create(editorRef.current, {
          value: text.content,
          language: text.syntax.toLowerCase(),
          theme: 'vs',
          readOnly: true,
        });
      } else {
        console.log("editorRef.current", editorRef.current);
        console.log("!editorInstance.current", !editorInstance.current)
      }
    };

    initializeEditor();

    return () => {
      isMounted = false;
      editorInstance.current?.dispose();
      editorInstance.current = null;
    };
  }, [textId]);

  const convertDate = (d: Date): string => {
    return `${d.getDate()}.${d.getMonth()}.${d.getFullYear()}`;
  }

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
  }

  return (
    <div className="reader">
      {text ?
        <div className="text">
          <div className="header">
            <div className="info">
              <img src="img/user_icon_black.svg" alt="user" />
              <div className="text-type-info">
                <p className="title">{text.title}</p>
                <div className="sub-info">
                  <Link to={`/profile/${encodeURIComponent(text.ownerName)}`}><p className="owner-name">{text.ownerName}</p></Link>
                  <p className="date">{convertDate(text.createdOn)}</p>
                  <p className="syntax">{text.syntax}</p>
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
              <Link to="/"><img src="img/download_black.svg" alt="download" /></Link>
              <Link to="/"><img src="img/delete_black.svg" alt="delete" /></Link>
            </div>
          </div>
          {text.content == "" &&  "Текст пуст! Автор пока что ничего сюда не добавил."}
          <div className="content" ref={editorRef} style={{ height: '80vh' }} />
        </div>
        :
        <div className="error">{error}</div>
      }
    </div>
  )
}

export default Reader;