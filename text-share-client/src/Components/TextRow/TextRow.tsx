import React from 'react'
import { Link } from 'react-router'
import { TextWithoutContentDto } from '../../Dtos'
import "./TextRow.css"

interface Props {
  text: TextWithoutContentDto,
  getIcon: (accessType: string) => string,
  includeOwner: boolean
}

const TextRow = ({ text, getIcon , includeOwner}: Props) => {
  const dateToString = (d: Date):string => {
    return `${d.getDate()}.${d.getMonth() + 1}.${d.getFullYear()}`;
  } 

  return (
    <tr>
      <td>
        <Link to={`/reader/${encodeURIComponent(text.id)}`} title={text.description}>{getIcon(text.accessType)}{text.title}</Link>
      </td>
      {includeOwner && 
        <td>
          <Link to={`/reader/${encodeURIComponent(text.id)}`} title={text.description}>{text.ownerName}</Link>
        </td>
      }
      <td>
        <Link to={`/reader/${encodeURIComponent(text.id)}`} title={text.createdOn.toDateString()}>{dateToString(text.createdOn)}</Link>
      </td>
      <td>
        <Link to={`/reader/${encodeURIComponent(text.id)}`} title={text.tags.join(' ')}>{text.tags.slice(0, 3).join(" ")} {text.tags.length > 3 && "..."}</Link>
      </td>
      <td>
        <Link to={`/reader/${encodeURIComponent(text.id)}`} title={text.description}>{text.syntax}</Link>
      </td>
      <td>
        <Link to={`/reader/${encodeURIComponent(text.id)}`} title={text.description}>{text.hasPassword ? (<p>есть</p>) : (<p>нету</p>)}</Link>
      </td>
    </tr>
  )
}

export default TextRow