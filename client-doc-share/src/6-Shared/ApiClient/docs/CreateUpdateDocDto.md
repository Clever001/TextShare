# CreateUpdateDocDto


## Properties

Name | Type | Description | Notes
------------ | ------------- | ------------- | -------------
**title** | **string** |  | [default to undefined]
**description** | **string** |  | [default to undefined]
**tags** | **Array&lt;string&gt;** |  | [default to undefined]
**roles** | [**{ [key: string]: UserDevRole; }**](UserDevRole.md) |  | [default to undefined]

## Example

```typescript
import { CreateUpdateDocDto } from './api';

const instance: CreateUpdateDocDto = {
    title,
    description,
    tags,
    roles,
};
```

[[Back to Model list]](../README.md#documentation-for-models) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to README]](../README.md)
