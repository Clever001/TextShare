# CommentsApi

All URIs are relative to *http://127.0.0.1:7285*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**clearComment**](#clearcomment) | **DELETE** /api/comments/{commentId} | |
|[**createComment**](#createcomment) | **POST** /api/comments | |
|[**getCommentById**](#getcommentbyid) | **GET** /api/comments/{commentId} | |
|[**searchComments**](#searchcomments) | **GET** /api/comments | |
|[**updateComment**](#updatecomment) | **PUT** /api/comments/{commentId} | |

# **clearComment**
> clearComment()


### Example

```typescript
import {
    CommentsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CommentsApi(configuration);

let commentId: number; // (default to undefined)

const { status, data } = await apiInstance.clearComment(
    commentId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **commentId** | [**number**] |  | defaults to undefined|


### Return type

void (empty response body)

### Authorization

[Bearer](../README.md#Bearer)

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

# **createComment**
> CommentDto createComment(createCommentDto)


### Example

```typescript
import {
    CommentsApi,
    Configuration,
    CreateCommentDto
} from './api';

const configuration = new Configuration();
const apiInstance = new CommentsApi(configuration);

let createCommentDto: CreateCommentDto; //

const { status, data } = await apiInstance.createComment(
    createCommentDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **createCommentDto** | **CreateCommentDto**|  | |


### Return type

**CommentDto**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**400** | Bad Request |  -  |
|**500** | Internal Server Error |  -  |
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **getCommentById**
> CommentDto getCommentById()


### Example

```typescript
import {
    CommentsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CommentsApi(configuration);

let commentId: number; // (default to undefined)

const { status, data } = await apiInstance.getCommentById(
    commentId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **commentId** | [**number**] |  | defaults to undefined|


### Return type

**CommentDto**

### Authorization

[Bearer](../README.md#Bearer)

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

# **searchComments**
> PaginatedResponseDtoOfCommentDto searchComments()


### Example

```typescript
import {
    CommentsApi,
    Configuration
} from './api';

const configuration = new Configuration();
const apiInstance = new CommentsApi(configuration);

let pageNumber: number; // (optional) (default to undefined)
let pageSize: number; // (optional) (default to undefined)
let parentId: number; // (optional) (default to undefined)
let documentId: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.searchComments(
    pageNumber,
    pageSize,
    parentId,
    documentId
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **pageNumber** | [**number**] |  | (optional) defaults to undefined|
| **pageSize** | [**number**] |  | (optional) defaults to undefined|
| **parentId** | [**number**] |  | (optional) defaults to undefined|
| **documentId** | [**string**] |  | (optional) defaults to undefined|


### Return type

**PaginatedResponseDtoOfCommentDto**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: Not defined
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**400** | Bad Request |  -  |
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **updateComment**
> CommentDto updateComment(updateCommentDto)


### Example

```typescript
import {
    CommentsApi,
    Configuration,
    UpdateCommentDto
} from './api';

const configuration = new Configuration();
const apiInstance = new CommentsApi(configuration);

let commentId: number; // (default to undefined)
let updateCommentDto: UpdateCommentDto; //

const { status, data } = await apiInstance.updateComment(
    commentId,
    updateCommentDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **updateCommentDto** | **UpdateCommentDto**|  | |
| **commentId** | [**number**] |  | defaults to undefined|


### Return type

**CommentDto**

### Authorization

[Bearer](../README.md#Bearer)

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

