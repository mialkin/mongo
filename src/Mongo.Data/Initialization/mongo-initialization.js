db = db.getSiblingDB("example");

db.createCollection("orders");

db.orders.insertMany([
    {
        "OrderId": 100,
        "Lines": [
            {
                "Position": 1,
                "ProductId": 4444
            },
            {
                "Position": 2,
                "ProductId": 4455
            }
        ]
    },
    {
        "OrderId": 101,
        "Lines": [
            {
                "Position": 1,
                "ProductId": 500
            }
        ]
    }
]);
