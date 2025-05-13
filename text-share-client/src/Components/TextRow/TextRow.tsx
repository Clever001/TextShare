import React from 'react'
import { Link } from 'react-router'
import { TextWithoutContentDto } from '../../Dtos'
import "./TextRow.css"

interface Props {
  text: TextWithoutContentDto,
  getIcon: (accessType: string) => string,
}

const TextRow = ({ text, getIcon }: Props) => {
  const dateToString = (d: Date):string => {
    return `${d.getDate()}.${d.getMonth() + 1}.${d.getFullYear()}`;
  } 

  return (
    <tr>
      <td>
        <Link to={`/text/${encodeURIComponent(text.title)}`}>{getIcon(text.accessType)}{text.title}</Link>
      </td>
      <td>
        <Link to={`/text/${encodeURIComponent(text.title)}`}>{text.ownerName}</Link>
      </td>
      <td>
        <Link to={`/text/${encodeURIComponent(text.title)}`}>{dateToString(text.createdOn)}</Link>
      </td>
      <td>
        <Link to={`/text/${encodeURIComponent(text.title)}`}>{text.tags.slice(0, 3).join(" ")} {text.tags.length > 3 && "..."}</Link>
      </td>
      <td>
        <Link to={`/text/${encodeURIComponent(text.title)}`}>{text.syntax}</Link>
      </td>
      <td>
        <Link to={`/text/${encodeURIComponent(text.title)}`}>{text.hasPassword ? (<p>есть</p>) : (<p>нету</p>)}</Link>
      </td>
    </tr>
  )
}

export default TextRow