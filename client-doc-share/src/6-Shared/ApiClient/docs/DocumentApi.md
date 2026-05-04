# DocumentApi

All URIs are relative to *http://127.0.0.1:7285*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**createDocument**](#createdocument) | **POST** /api/documents | |
|[**deleteDocumentById**](#deletedocumentbyid) | **DELETE** /api/documents/{docId} | |
|[**getDocumentById**](#getdocumentbyid) | **GET** /api/documents/{docId} | |
|[**searchDocuments**](#searchdocuments) | **GET** /api/documents | |
|[**updateDocument**](#updatedocument) | **PUT** /api/documents/{docId} | |

# **createDocument**
> DocumentDto createDocument(createUpdateDocDto)


### Example

```typescript
import {
    DocumentApi,
    Configuration,
    CreateUpdateDocDto
} from './api';

const configuration = new Configuration();
const apiInstance = new DocumentApi(configuration);

let createUpdateDocDto: CreateUpdateDocDto; //

const { status, data } = await apiInstance.createDocument(
    createUpdateDocDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **createUpdateDocDto** | **CreateUpdateDocDto**|  | |


### Return type

**DocumentDto**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**400** | Bad Request |  -  |
|**500** | Internal Server Error |  -  |
|**201** | Created |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **deleteDocumentById**
> deleteDocumentById()


### Example

```typescript
import {
    DocumentApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DocumentApi(configuration);

let docId: string; // (default to undefined)

const { status, data } = await apiInstance.deleteDocumentById(
    docId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **docId** | [**string**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**400** | Bad Request |  -  |
|**404** | Not Found |  -  |
|**403** | Forbidden |  -  |
|**500** | Internal Server Error |  -  |
|**204** | No Content |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getDocumentById**
> DocumentDto getDocumentById()


### Example

```typescript
import {
    DocumentApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new DocumentApi(configuration);

let docId: string; // (default to undefined)

const { status, data } = await apiInstance.getDocumentById(
    docId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **docId** | [**string**] |  | defaults to undefined|


### Return type

**DocumentDto**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**400** | Bad Request |  -  |
|**404** | Not Found |  -  |
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **searchDocuments**
> PaginatedResponseDtoOfDocumentDto searchDocuments()


### Example

```typescript
import {
    DocumentApi,
    Configuration,
    SearchAccountsPageNumberParameter,
    SearchAccountsPageSizeParameter
} from './api';

const configuration = new Configuration();
const apiInstance = new DocumentApi(configuration);

let sortBy: string; // (optional) (default to undefined)
let sortAscending: boolean; // (optional) (default to undefined)
let pageNumber: SearchAccountsPageNumberParameter; // (optional) (default to undefined)
let pageSize: SearchAccountsPageSizeParameter; // (optional) (default to undefined)
let title: string; // (optional) (default to undefined)
let tags: Array<string>; // (optional) (default to undefined)
let fromDate: string; // (optional) (default to undefined)
let toDate: string; // (optional) (default to undefined)
let ownerName: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.searchDocuments(
    sortBy,
    sortAscending,
    pageNumber,
    pageSize,
    title,
    tags,
    fromDate,
    toDate,
    ownerName
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **sortBy** | [**string**] |  | (optional) defaults to undefined|
| **sortAscending** | [**boolean**] |  | (optional) defaults to undefined|
| **pageNumber** | [**SearchAccountsPageNumberParameter**] |  | (optional) defaults to undefined|
| **pageSize** | [**SearchAccountsPageSizeParameter**] |  | (optional) defaults to undefined|
| **title** | [**string**] |  | (optional) defaults to undefined|
| **tags** | **Array&lt;string&gt;** |  | (optional) defaults to undefined|
| **fromDate** | [**string**] |  | (optional) defaults to undefined|
| **toDate** | [**string**] |  | (optional) defaults to undefined|
| **ownerName** | [**string**] |  | (optional) defaults to undefined|


### Return type

**PaginatedResponseDtoOfDocumentDto**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**400** | Bad Request |  -  |
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateDocument**
> DocumentDto updateDocument(createUpdateDocDto)


### Example

```typescript
import {
    DocumentApi,
    Configuration,
    CreateUpdateDocDto
} from './api';

const configuration = new Configuration();
const apiInstance = new DocumentApi(configuration);

let docId: string; // (default to undefined)
let createUpdateDocDto: CreateUpdateDocDto; //

const { status, data } = await apiInstance.updateDocument(
    docId,
    createUpdateDocDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **createUpdateDocDto** | **CreateUpdateDocDto**|  | |
| **docId** | [**string**] |  | defaults to undefined|


### Return type

**DocumentDto**

### Authorization

No authorization required

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**400** | Bad Request |  -  |
|**404** | Not Found |  -  |
|**403** | Forbidden |  -  |
|**500** | Internal Server Error |  -  |
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

