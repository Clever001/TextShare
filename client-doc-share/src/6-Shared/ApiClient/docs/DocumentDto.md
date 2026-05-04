# DocumentDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**id** | **string** |  | [default to undefined]
**title** | **string** |  | [default to undefined]
**description** | **string** |  | [default to undefined]
**createdOn** | **string** |  | [default to undefined]
**ownerName** | **string** |  | [default to undefined]
**tags** | **Array&lt;string&gt;** |  | [default to undefined]
**userNamesToRoles** | [**{ [key: string]: UserDevRole; }**](UserDevRole.md) |  | [default to undefined]

## Example

```typescript
import { DocumentDto } from './api';

const instance: DocumentDto = {
    id,
    title,
    description,
    createdOn,
    ownerName,
    tags,
    userNamesToRoles,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
