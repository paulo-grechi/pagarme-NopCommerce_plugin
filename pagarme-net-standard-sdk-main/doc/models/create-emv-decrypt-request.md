
# Create Emv Decrypt Request

## Structure

`CreateEmvDecryptRequest`

## Fields

| Name | Type | Tags | Description |
|  --- | --- | --- | --- |
| `IccData` | `string` | Required | - |
| `CardSequenceNumber` | `string` | Required | - |
| `Data` | [`Models.CreateEmvDataDecryptRequest`](../../doc/models/create-emv-data-decrypt-request.md) | Required | - |
| `Poi` | [`Models.CreateCardPaymentContactlessPOIRequest`](../../doc/models/create-card-payment-contactless-poi-request.md) | Optional | - |

## Example (as JSON)

```json
{
  "icc_data": "icc_data6",
  "card_sequence_number": "card_sequence_number0",
  "data": {
    "cipher": "cipher4",
    "tags": [
      {
        "tag": "tag9",
        "lenght": "lenght7",
        "value": "value7"
      }
    ],
    "dukpt": {
      "ksn": "ksn0"
    }
  },
  "poi": {
    "system_name": "system_name4",
    "model": "model2",
    "provider": "provider4",
    "serial_number": "serial_number2",
    "version_number": "version_number6"
  }
}
```

