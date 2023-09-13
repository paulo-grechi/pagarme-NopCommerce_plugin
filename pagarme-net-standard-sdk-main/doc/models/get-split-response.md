
# Get Split Response

Split response

## Structure

`GetSplitResponse`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `Type` | `string` | Optional | Type |
| `Amount` | `int?` | Optional | Amount |
| `Recipient` | [`Models.GetRecipientResponse`](../../doc/models/get-recipient-response.md) | Optional | Recipient |
| `GatewayId` | `string` | Optional | The split rule gateway id |
| `Options` | [`Models.GetSplitOptionsResponse`](../../doc/models/get-split-options-response.md) | Optional | - |
| `Id` | `string` | Optional | - |

## Example (as JSON)

```json
{
  "type": "type0",
  "amount": 46,
  "recipient": {
    "id": "id8",
    "name": "name8",
    "email": "email8",
    "document": "document8",
    "description": "description2"
  },
  "gateway_id": "gateway_id0",
  "options": {
    "liable": false,
    "charge_processing_fee": false,
    "charge_remainder_fee": "charge_remainder_fee0"
  }
}
```

