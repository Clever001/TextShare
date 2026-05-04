# CommentDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | [**SearchCommentsParentIdParameter**](SearchCommentsParentIdParameter.md) |  | [default to undefined]
**content** | **string** |  | [default to undefined]
**parentId** | [**CommentDtoParentId**](CommentDtoParentId.md) |  | [default to undefined]
**documentId** | **string** |  | [default to undefined]
**authorId** | **string** |  | [default to undefined]
**authorName** | **string** |  | [default to undefined]
**hasChildren** | **boolean** |  | [default to undefined]
**isDevelopmentComment** | **boolean** |  | [default to undefined]
**createdOn** | **string** |  | [default to undefined]

## Example

```typescript
import { CommentDto } from './api';

const instance: CommentDto = {
    id,
    content,
    parentId,
    documentId,
    authorId,
    authorName,
    hasChildren,
    isDevelopmentComment,
    createdOn,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
