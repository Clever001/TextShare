import { useCallback, useContext, useEffect, useLayoutEffect, useReducer, useRef, useState } from "react";
import DocumentWithVersionsFeature from "../3-Features/DocumentWithVersions/DocumentWithVersions";
import DocumentFeature from "../3-Features/Document/DocumentFeature";
import { UserContext } from "../1-Processes/UserContext";
import { useNavigate } from "react-router-dom";
import { ROUTES } from "../Router/Router";
import type { DeleteVersionRequest, MessageDto, RenamedVersionRequest, RenameVersionRequest, SendVersionRequest, Version, VersionDto, VersionsListDto, VersionState } from "../6-Shared/Dtos";

export default function DocumentPage() {
  const [showDocumentVersions, setShowDocumentVersions] = useState<boolean>(false);
  const userContext = useContext(UserContext);
  const navigate = useNavigate();

  const toggleDocumentVersionsBlock = (): void => {
    setShowDocumentVersions(!showDocumentVersions);
    setDocVersionState(null);
  }

  // Check for null user
  useEffect(() => {
    const nullableUser = userContext.getUser();
    if (nullableUser === null) {
      navigate(ROUTES.MAIN);
    }
  }, []);

  // Versions web socket and list init.
  const versionsSocket = useRef<WebSocket | null>(null);
  const [versionsList, setVersionsList] = useState<Version[]>([]);
  const [docVersionState, setDocVersionState] = useState<Uint8Array<ArrayBufferLike> | null>(null);
  const consumeCreateVersion = (newVersion: Version) => {
    setVersionsList(prevVersions => [...prevVersions, newVersion]);
  };
  const consumeUpdateVersion = (updateVersion: Version): void => {
    console.log("update consumed: ", updateVersion);
    setVersionsList(prevVersions =>
      prevVersions.map(version =>
        version.id === updateVersion.id ? updateVersion : version
      )
    );
  };
  const consumeDeleteVersion = (deleteVersionId: string): void => {
    setVersionsList(prevVersions =>
      prevVersions.filter(version => version.id !== deleteVersionId)
    );
  };
  const consumeVersionState = (versionState: string): void => {
    const update = new Uint8Array(atob(versionState).split('').map(char => char.charCodeAt(0)));
    setDocVersionState(update);
  }
  const publishCreateVersion = (versionName: string): void => {
    if (versionsSocket.current) {
      const createVersionDto: VersionDto = {
        type: "createNewVersion",
        id: "",
        name: versionName,
        createdTime: Math.floor(Date.now() / 1000)
      };

      versionsSocket.current.send(JSON.stringify(createVersionDto));
    }
  }
  const publishSendVersionRequest = (versionId: string): void => {
    if (versionsSocket.current) {
      const sendVersionRequest: SendVersionRequest = {
        type: "sendCertainVersion",
        versionId: versionId
      };

      versionsSocket.current.send(JSON.stringify(sendVersionRequest));
    }
  }
  const publishDeleteVersionRequest = (versionId: string) : void => {
    if (versionsSocket.current) {
      const deleteDto: DeleteVersionRequest = {
        type: "deleteCertainVersion",
        versionId: versionId
      }

      versionsSocket.current.send(JSON.stringify(deleteDto));
    }
  }
  const publishRenameVersionRequest = (versionId: string, newName: string): void => {
    if (versionsSocket.current) {
      const renameDto: RenameVersionRequest = {
        type: "renameCertainVersion",
        versionId: versionId,
        newVersionName: newName,
      }

      console.log("Sended rename info: ", renameDto);
      versionsSocket.current.send(JSON.stringify(renameDto));
    }
  }
  useEffect(() => {
    const initVersionsSocket = () => {
      const socket: WebSocket = new WebSocket(import.meta.env.VITE_CUSTOM_WS_URL as string);
      socket.onmessage = ((event) => {
        const message = JSON.parse(event.data) as MessageDto;
        switch (message.type) {
          case "versionsList":
            const parsedList = message as VersionsListDto;
            setVersionsList(parsedList.versions);
            break;
          case "newVersionCreated":
            const newVersion = message as VersionDto;
            const parsedVersion: Version = {
              id: newVersion.id,
              name: newVersion.name,
              createdTime: newVersion.createdTime
            };
            consumeCreateVersion(parsedVersion);
            break;
          case "sendedCertainVersion":
            const versionState = message as VersionState;
            consumeVersionState(versionState.versionState);
            break;
          case "deletedCertainVersion":
            const deleteRequest = message as DeleteVersionRequest;
            consumeDeleteVersion(deleteRequest.versionId);
            break;
          case "renamedCertainVersion":
            const renameRequst = message as RenamedVersionRequest;
            const updateVersion: Version = {
              id: renameRequst.id,
              name: renameRequst.name,
              createdTime: renameRequst.createdTime
            };
            consumeUpdateVersion(updateVersion);
            break;
        }
      });
      versionsSocket.current = socket;
    }
    initVersionsSocket();
    return () => {
      if (versionsSocket.current) {
        versionsSocket.current.onopen = null;
        versionsSocket.current.onmessage = null;
        versionsSocket.current.onerror = null;
        versionsSocket.current.onclose = null;
        if (versionsSocket.current.readyState === WebSocket.OPEN) {
          versionsSocket.current.close(1000, "Normal Closure");
        }
        versionsSocket.current = null;
      }
    };
  }, [])

  return <>
    {showDocumentVersions ?
      <DocumentWithVersionsFeature
        toggleDocumentVersionsBlock={toggleDocumentVersionsBlock}
        versions={versionsList}
        handleSwitchVersion={publishSendVersionRequest}
        handleDeleteVersion={publishDeleteVersionRequest}
        handleRenameVersion={publishRenameVersionRequest}
        documentState={docVersionState} />
      :
      <DocumentFeature
        toggleDocumentVersionsBlock={toggleDocumentVersionsBlock}
        onCreateNewVersion={publishCreateVersion} />
    }
  </>
}