import { useState } from "react";
import type { Version } from "../../6-Shared/Dtos";
import './VersionsListWidget.css';

type Props = {
  versions: Version[];
  onSwitchVersion: (versionId: string) => void;
  onDeleteVersion: (versionId: string) => void;
  onRenameVersion: (versionId: string, newName: string) => void;
  onSetCurrentVersion: (versionId: string) => void;
};

export default function VersionsListWidget({
  versions, onSwitchVersion, onDeleteVersion, onRenameVersion, onSetCurrentVersion
}: Props) {
  const sortedVersions = [...versions];
  sortedVersions.sort((prev, next) => next.createdTime - prev.createdTime);

  const [editingId, setEditingId] = useState<string | null>(null);
  const [editValue, setEditValue] = useState('');

  const startEditing = (version: Version) => {
    setEditingId(version.id);
    setEditValue(version.name);
  };

  const saveEditing = (id: string) => {
    if (editValue.trim() !== '') {
      onRenameVersion(id, editValue.trim());
    } else {
      alert("Название не может быть пустым.");
    }
    setEditingId(null);
  };

  const cancelEditing = () => {
    setEditingId(null);
  };

  const handleKeyDown = (e: React.KeyboardEvent, id: string) => {
    if (e.key === 'Enter') saveEditing(id);
    if (e.key === 'Escape') cancelEditing();
  };

  const unixUtcToDate = (unixTime: number): Date => {
    return new Date(unixTime * 1000);
  }

  return (
    <div className="version-list-widget">
      {/* <button className="command-btn">
        Установить версию как текущую
      </button> */}

      <div className="versions-container">
        {sortedVersions.length === 0 ? (
          <p className="no-versions">Нет сохранённых версий</p>
        ) : (
          sortedVersions.map(version => (
            <div key={version.id} className="version-item">
              <div className="version-info">
                {editingId === version.id ? (
                  <input
                    type="text"
                    value={editValue}
                    onChange={e => setEditValue(e.target.value)}
                    onBlur={() => saveEditing(version.id)}
                    onKeyDown={e => handleKeyDown(e, version.id)}
                    autoFocus
                    className="version-title-input"
                  />
                ) : (
                  <div className="version-title-wrapper">
                    <span className="version-title">{version.name}</span>
                    <button
                      className="edit-title-btn"
                      onClick={() => startEditing(version)}
                      title="Редактировать название"
                    >
                      ✏️
                    </button>
                  </div>
                )}
                <span className="version-date">
                  {unixUtcToDate(version.createdTime).toLocaleString()}
                </span>
              </div>

              <div className="version-actions">
                <button
                  className="switch-btn"
                  onClick={() => onSwitchVersion(version.id)}
                  title="Переключиться на эту версию"
                >
                  👁️
                </button>
                <button
                  className="delete-btn"
                  onClick={() => onDeleteVersion(version.id)}
                  title="Удалить версию"
                >
                  🗑️
                </button>
                <button
                  className="set-current-btn"
                  onClick={() => onSetCurrentVersion(version.id)}
                  title="Установить как текущую версию"
                >
                  ✅
                </button>
              </div>
            </div>
          ))
        )}
      </div>
    </div>
  )
}