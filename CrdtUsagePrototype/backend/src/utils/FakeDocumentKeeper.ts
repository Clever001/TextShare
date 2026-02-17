import { Mutex } from "async-mutex";
import { DocumentKeeper } from "./DocumentKeeper";
import { DocumentVersionMetadata, Document } from "./Dtos";

export class FakeDocumentKeeper implements DocumentKeeper {
  private readonly versionNameToDocument: Map<string, Document>;
  private readonly versionNameToMetadata: Map<string, DocumentVersionMetadata>;
  private readonly headVersion: Document;

  private readonly mutex: Mutex;

  public constructor() {
    this.versionNameToDocument = new Map();
    this.versionNameToMetadata = new Map();
    this.headVersion = {
      htmlText: ''
    };
    this.mutex = new Mutex();
  }

  private waitForCommunicationDelay() {
    const delay = Math.random() * 300 + 200;
    return new Promise((resolve) => setTimeout(resolve, delay));
  }

  public async getHeadVersionOfDocument(): Promise<Document> {
    await this.waitForCommunicationDelay();

    const release = await this.mutex.acquire();
    try {
      return structuredClone(this.headVersion);
    } finally {
      await release();
    }
  }

  public async getDocumentVersionsMetadata(): Promise<DocumentVersionMetadata[]> {
    await this.waitForCommunicationDelay();

    const release = await this.mutex.acquire();
    try {
      return Array.from(this.versionNameToMetadata.values());
    } finally {
      await release();
    }
  }

  public async getDocumentByVersionName(versionName: string): Promise<Document> {
    await this.waitForCommunicationDelay();

    const release = await this.mutex.acquire();
    try {
      const document = this.versionNameToDocument.get(versionName);
      if (document === undefined) {
        throw new Error(`Document version ${versionName} not found`);
      }
      return structuredClone(document);
    } finally {
      await release();
    }
  }

  public async saveHeadVersionOfDocument(document: Document): Promise<void> {
    await this.waitForCommunicationDelay();

    const release = await this.mutex.acquire();
    try {
      this.headVersion.htmlText = document.htmlText;
    } finally {
      await release();
    }
  }

  public async saveNewVersionOfDocument(
    document: Document, 
    version: DocumentVersionMetadata
  ): Promise<void> {
    await this.waitForCommunicationDelay();

    const release = await this.mutex.acquire();
    try {
      this.versionNameToDocument.set(version.versionName, structuredClone(document));
      this.versionNameToMetadata.set(version.versionName, structuredClone(version));
    } finally {
      await release();
    }
  }

  public async removeDocumentVersionsAfter(versionName: string): Promise<void> {
    await this.waitForCommunicationDelay();

    const release = await this.mutex.acquire();
    try {
      const requiredVersion = this.versionNameToMetadata.get(versionName);
      if (requiredVersion === undefined) {
        throw new Error(`Document version ${versionName} not found`);
      }

      const allVersions = this.versionNameToMetadata.values();
      const versionsToRemove = [];
      for (const version of allVersions) {
        if (version.creationTime > requiredVersion.creationTime) {
          versionsToRemove.push(version.versionName);
        }
      }

      for (const versionName of versionsToRemove) {
        this.versionNameToMetadata.delete(versionName);
        this.versionNameToDocument.delete(versionName);
      }

      var latestVersion: DocumentVersionMetadata | null = null;
      for (const version of this.versionNameToMetadata.values()) {
        if (latestVersion === null || version.creationTime > latestVersion.creationTime) {
          latestVersion = version;
        }
      }

      if (latestVersion === null) {
        this.headVersion.htmlText = '';
      } else {
        const document = this.versionNameToDocument.get(latestVersion.versionName);
        if (document === undefined) {
          throw new Error(`Document version ${latestVersion.versionName} not found`);
        }
        this.headVersion.htmlText = document.htmlText;
      }
    } finally {
      await release();
    }
  }

  public async removeSpecificDocumentVersion(versionName: string): Promise<void> {
    await this.waitForCommunicationDelay();

    const release = await this.mutex.acquire();
    try {
      if (!this.versionNameToMetadata.has(versionName)) {
        throw new Error(`Document version ${versionName} not found`);
      }
      this.versionNameToMetadata.delete(versionName);
      this.versionNameToDocument.delete(versionName);
    } finally {
      await release();
    }
  }
}