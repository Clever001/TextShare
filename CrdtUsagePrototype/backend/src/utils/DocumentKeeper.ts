import { DocumentVersionMetadata, Document } from "./Dtos";

export interface DocumentKeeper {
  getHeadVersionOfDocument(): Promise<Document>;
  getDocumentVersionsMetadata(): Promise<DocumentVersionMetadata[]>;
  getDocumentByVersionName(versionName: string): Promise<Document>;

  saveHeadVersionOfDocument(document: Document): Promise<void>;
  saveNewVersionOfDocument(
    document: Document, 
    version: DocumentVersionMetadata
  ): Promise<void>;

  removeDocumentVersionsAfter(versionName: string): Promise<void>;
  removeSpecificDocumentVersion(versionName: string): Promise<void>;
}