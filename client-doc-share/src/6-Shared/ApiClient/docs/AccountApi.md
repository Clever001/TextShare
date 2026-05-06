# AccountApi

All URIs are relative to *http://127.0.0.1:7285*

|Method | HTTP request | Description|
|------------- | ------------- | -------------|
|[**getAccountsThatStartsWith**](#getaccountsthatstartswith) | **GET** /api/accounts/startsWith | |
|[**login**](#login) | **POST** /api/accounts/login | |
|[**register**](#register) | **POST** /api/accounts/register | |
|[**searchAccounts**](#searchaccounts) | **GET** /api/accounts | |
|[**updateAccountInfo**](#updateaccountinfo) | **PUT** /api/accounts | |

# **getAccountsThatStartsWith**
> Array<UserWithoutTokenDto> getAccountsThatStartsWith()


### Example

```typescript
import {
    AccountApi,
    Configuration,
    GetAccountsThatStartsWithTakeParameter
} from './api';

const configuration = new Configuration();
const apiInstance = new AccountApi(configuration);

let take: GetAccountsThatStartsWithTakeParameter; // (optional) (default to undefined)
let userName: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.getAccountsThatStartsWith(
    take,
    userName
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **take** | [**GetAccountsThatStartsWithTakeParameter**] |  | (optional) defaults to undefined|
| **userName** | [**string**] |  | (optional) defaults to undefined|


### Return type

**Array<UserWithoutTokenDto>**

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

# **login**
> UserWithTokenDto login(loginDto)


### Example

```typescript
import {
    AccountApi,
    Configuration,
    LoginDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AccountApi(configuration);

let loginDto: LoginDto; //

const { status, data } = await apiInstance.login(
    loginDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **loginDto** | **LoginDto**|  | |


### Return type

**UserWithTokenDto**

### Authorization

[Bearer](../README.md#Bearer)

### HTTP request headers

 - **Content-Type**: application/json, text/json, application/*+json
 - **Accept**: application/json


### HTTP response details
| Status code | Description | Response headers |
|-------------|-------------|------------------|
|**400** | Bad Request |  -  |
|**401** | Unauthorized |  -  |
|**200** | OK |  -  |

[[Back to top]](#) [[Back to API list]](../README.md#documentation-for-api-endpoints) [[Back to Model list]](../README.md#documentation-for-models) [[Back to README]](../README.md)

# **register**
> UserWithTokenDto register(registerDto)


### Example

```typescript
import {
    AccountApi,
    Configuration,
    RegisterDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AccountApi(configuration);

let registerDto: RegisterDto; //

const { status, data } = await apiInstance.register(
    registerDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **registerDto** | **RegisterDto**|  | |


### Return type

**UserWithTokenDto**

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

# **searchAccounts**
> PaginatedResponseDtoOfUserWithoutTokenDto searchAccounts()


### Example

```typescript
import {
    AccountApi,
    Configuration,
    SearchAccountsPageNumberParameter,
    SearchAccountsPageSizeParameter
} from './api';

const configuration = new Configuration();
const apiInstance = new AccountApi(configuration);

let pageNumber: SearchAccountsPageNumberParameter; // (optional) (default to undefined)
let pageSize: SearchAccountsPageSizeParameter; // (optional) (default to undefined)
let userName: string; // (optional) (default to undefined)

const { status, data } = await apiInstance.searchAccounts(
    pageNumber,
    pageSize,
    userName
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **pageNumber** | [**SearchAccountsPageNumberParameter**] |  | (optional) defaults to undefined|
| **pageSize** | [**SearchAccountsPageSizeParameter**] |  | (optional) defaults to undefined|
| **userName** | [**string**] |  | (optional) defaults to undefined|


### Return type

**PaginatedResponseDtoOfUserWithoutTokenDto**

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

# **updateAccountInfo**
> UserWithTokenDto updateAccountInfo(updateUserDto)


### Example

```typescript
import {
    AccountApi,
    Configuration,
    UpdateUserDto
} from './api';

const configuration = new Configuration();
const apiInstance = new AccountApi(configuration);

let updateUserDto: UpdateUserDto; //

const { status, data } = await apiInstance.updateAccountInfo(
    updateUserDto
);
```

### Parameters

|Name | Type | Description  | Notes|
|------------- | ------------- | ------------- | -------------|
| **updateUserDto** | **UpdateUserDto**|  | |


### Return type

**UserWithTokenDto**

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

