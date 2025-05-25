import React from 'react'
import './NotFound.css'

type Props = {}

const NotFound = (props: Props) => {
  return (
    <div className="not-found">
      <div className="title">Страница не найдена</div>
      <div className="content">Запрашиваемая страница не была найдена.</div>
    </div>
  )
}

export default NotFound