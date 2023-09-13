
# Get Token Response

Token data

## Structure

`GetTokenResponse`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `Id` | `string` | Optional | - |
| `Type` | `string` | Optional | - |
| `CreatedAt` | `DateTime?` | Optional | - |
| `ExpiresAt` | `string` | Optional | - |
| `Card` | [`Models.GetCardTokenResponse`](../../doc/models/get-card-token-response.md) | Optional | - |

## Example (as JSON)

```json
{
  "id": "id0",
  "type": "type0",
  "created_at": "2016-03-13T12:52:32.123Z",
  "expires_at": "expires_at6",
  "card": {
    "last_four_digits": "last_four_digits2",
    "holder_name": "holder_name2",
    "holder_document": "holder_document0",
    "exp_month": 228,
    "exp_year": 68
  }
}
```

