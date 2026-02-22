import { VersionDto } from "./Dtos";
import { Version, VersionToDocument } from "./Models";
import { randomUUID } from 'crypto';
import { YjsDocumentKeeper } from "./YjsDocumentKeeper";


export class VersionsKeeper {
  private readonly versionsMap = new Map<string, VersionToDocument>();

  public createNewVersion(dto: VersionDto, document: YjsDocumentKeeper) {
    let guid = randomUUID();
    while (this.versionsMap.has(guid)) {
      guid = randomUUID();
    }
    dto.id = guid;
    const pair: VersionToDocument = {
      version: {
        id: dto.id,
        name: dto.name,
        createdTime: dto.createdTime,
      },
      documentState: document.getDocumentStateAsUpdate(),
    };
    this.versionsMap.set(guid, pair);
  }

  public getVersion(id: string): VersionToDocument | null {
    return this.versionsMap.get(id) ?? null;
  }

  public getAllVersions(): Version[] {
    const versionsList: Version[] = [];

    for (const pair of this.versionsMap.values()) {
      versionsList.push(pair.version);
    }

    return versionsList;
  }

  public updateVersionName(id: string, name: string): boolean {
    if (this.versionsMap.has(id)) {
      const currentPair = this.versionsMap.get(id)!;
      currentPair.version.name = name;
      this.versionsMap.set(id, currentPair);
      return true;
    }
    return false;
  }

  public deleteVersion(id: string): boolean {
    return this.versionsMap.delete(id);
  }
}