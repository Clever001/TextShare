import * as Y from 'yjs';
import * as syncProtocol from 'y-protocols/sync';
import * as awarenessProtocol from 'y-protocols/awareness';
import * as encoding from 'lib0/encoding';
import * as decoding from 'lib0/decoding';

export class YjsDocumentKeeper {
  private readonly doc: Y.Doc;
  private static readonly messageSync = 0
  private static readonly messageQueryAwareness = 3
  private static readonly messageAwareness = 1
  private static readonly messageAuth = 2

  private constructor(doc: Y.Doc) {
    this.doc = doc;
  }

  public static createEmptyDocument(): YjsDocumentKeeper {
    return new YjsDocumentKeeper(new Y.Doc());
  }

  public static createFromStateAsUpdate(state: Uint8Array<ArrayBufferLike>) {
    const doc = new Y.Doc();
    Y.applyUpdateV2(doc, state);
    return new YjsDocumentKeeper(doc);
  }

  // public updateDocument(update: Buffer): Buffer | null {
  //   const decoder = decoding.createDecoder(new Uint8Array(update));
  //   const encoder = encoding.createEncoder();
  //   const messageType = decoding.readVarUint(decoder);

  //   if (messageType === this.messageSync) {
  //     encoding.writeVarUint(encoder, this.messageSync);
  //     syncProtocol.readSyncMessage(decoder, encoder, this.doc, null);

  //     if (encoding.length(encoder) > 1) {
  //       return Buffer.from(encoding.toUint8Array(encoder));
  //     }
  //   }

  //   return null;
  // }

  public updateDocument(update: Uint8Array): { response: Uint8Array | null, shouldBroadcast: boolean } {
    const decoder = decoding.createDecoder(update);
    const encoder = encoding.createEncoder();
    const messageType = decoding.readVarUint(decoder);

    let shouldBroadcast = false;

    if (messageType === YjsDocumentKeeper.messageSync) {
      encoding.writeVarUint(encoder, YjsDocumentKeeper.messageSync);

      // readSyncMessage возвращает тип сообщения синхронизации:
      // 0 = SyncStep1 (Запрос), 1 = SyncStep2 (Ответ/Данные), 2 = Update (Данные)
      const syncType = syncProtocol.readSyncMessage(decoder, encoder, this.doc, null);

      // Если клиент прислал данные (Step 2 или Update), их НУЖНО разослать остальным
      if (syncType === syncProtocol.messageYjsSyncStep2 || syncType === syncProtocol.messageYjsUpdate) {
        shouldBroadcast = true;
      }
    }

    // Также обычно рассылают сообщения Awareness (курсоры)
    if (messageType === YjsDocumentKeeper.messageAwareness) {
      shouldBroadcast = true;
    }

    return {
      response: encoding.length(encoder) > 1 ? encoding.toUint8Array(encoder) : null,
      shouldBroadcast
    };
  }

  public getDocumentStateUpdateMessage(): Buffer {
    const encoder = encoding.createEncoder();
  
    encoding.writeVarUint(encoder, YjsDocumentKeeper.messageSync);
    syncProtocol.writeSyncStep1(encoder, this.doc);
    
    return Buffer.from(encoding.toUint8Array(encoder));
  }

  public getDocumentStateAsUpdate(): Uint8Array<ArrayBufferLike> {
    return Y.encodeStateAsUpdateV2(this.doc);
  }
}