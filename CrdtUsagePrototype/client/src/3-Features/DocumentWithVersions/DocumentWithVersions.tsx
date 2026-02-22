import { EditorWidget } from "../../4-Widgets/EditorWidget/EditorWidget";
import type { Version } from "../../6-Shared/Dtos";
import Document from '@tiptap/extension-document'
import Paragraph from '@tiptap/extension-paragraph'
import Text from '@tiptap/extension-text'
import Bold from '@tiptap/extension-bold'
import Italic from '@tiptap/extension-italic'
import Underlined from '@tiptap/extension-underline'
import Strike from '@tiptap/extension-strike'
import Code from '@tiptap/extension-code'
import Blockquote from '@tiptap/extension-blockquote'
import Image from '@tiptap/extension-image'
import Heading from '@tiptap/extension-heading'
import { useEditor } from '@tiptap/react'
import VersionsListWidget from "../../4-Widgets/VersionsListWidget/VersionsListWidget";
import './DocumentWithVersions.css';
import * as Y from "yjs";
import { useEffect, useState } from "react";
import Collaboration from "@tiptap/extension-collaboration";


type Props = {
  toggleDocumentVersionsBlock: () => void;
  versions: Version[];
  documentState: Uint8Array<ArrayBufferLike> | null;
  handleSwitchVersion: (versionId: string) => void;
  handleDeleteVersion: (versionId: string) => void;
  handleRenameVersion: (versionId: string, newName: string) => void;
  handleSetCurrentVersion: (versionId: string) => void;
};

export default function DocumentWithVersionsFeature({ 
  toggleDocumentVersionsBlock, versions, documentState, 
  handleSwitchVersion, handleDeleteVersion, handleRenameVersion,
  handleSetCurrentVersion
}: Props) {
  const [yjsDoc, setYjsDoc] = useState<Y.Doc | null>(null);

  useEffect(() => {
    const doc = new Y.Doc();
    if (documentState) {
      Y.applyUpdateV2(doc, documentState);
    }
    setYjsDoc(doc);

    return () => {
      yjsDoc?.destroy();
      setYjsDoc(null);
    };
  }, [documentState]);

  const editor = useEditor({
    extensions: [
      Document,
      Paragraph,
      Text,
      Bold,
      Italic,
      Underlined,
      Strike,
      Heading.configure({ levels: [1, 2, 3, 4, 5, 6] }),
      Code,
      Blockquote,
      Image,
      ...(yjsDoc && documentState !== null ?
        [
          Collaboration.configure({
            document: yjsDoc,
          }),
        ] : []
      )
    ],
    editable: false,
    content: documentState === null ? "Выберите версию документа" : undefined,
  }, [yjsDoc]);


  return (
    <div className="document-with-versions">
      <div className="document-left">
        <EditorWidget
          editor={editor}
          changeFormatting={(opt) => { }}
          addImage={(event) => { }}
          isEditing={false}
          toggleDocumentVersionsBlock={toggleDocumentVersionsBlock}
          onCreateNewVersion={null} />
      </div>
      <div className="versions-right">
        <VersionsListWidget
          versions={versions}
          onSwitchVersion={handleSwitchVersion}
          onDeleteVersion={handleDeleteVersion}
          onRenameVersion={handleRenameVersion}
          onSetCurrentVersion={handleSetCurrentVersion}
        />
      </div>
    </div>
  )
}