import Cookies from 'js-cookie';
import React, { useEffect, useRef, useState } from 'react'
import { Link, useParams } from 'react-router-dom';
import { SearchTextById } from '../../Services/API/TextSearchService';
import { TextWithContentDto } from '../../Dtos';
import * as monaco from 'monaco-editor';
import './Reader.css'

type Props = {}

const Reader = (props: Props) => {
  const {textId} = useParams();
  const [error, setError] = useState<string | null>(null);
  const [text, setText] = useState<TextWithContentDto | null>(null);

  const getText = async() : Promise<TextWithContentDto | null> => {
    const token: string | null = Cookies.get('token') ?? null;

    if (typeof(textId) === "undefined") {
      setError('Текста c таким id нет!');
      setText(null);
      return null;
    }

    const result = await SearchTextById(textId, token);

    if (Array.isArray(result)) {
      setError(result[0]);
      setText(null);
      return null;
    }

    setError(null);
    setText(result);
    return result;
  }

  useEffect(() => {
    getText();
  }, [textId]);

  const convertDate = (d: Date) :string => {
    return `${d.getDate()}.${d.getMonth()}.${d.getFullYear()}`;
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
              <Link to="/"><img src="img/copy_black.svg" alt="copy" /></Link>
              <Link to="/"><img src="img/edit_black.svg" alt="edit" /></Link>
              <Link to="/"><img src="img/download_black.svg" alt="download" /></Link>
              <Link to="/"><img src="img/delete_black.svg" alt="delete" /></Link>
            </div>
          </div>
          <textarea className="content" value={text.content}/>
        </div>
      :
        <div className="error">{error}</div>
      }
    </div>
  )
}

export default Reader