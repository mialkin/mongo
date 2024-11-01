db = db.getSiblingDB("example");

db.createCollection("orders");

db.orders.createIndex({"CartId": 1}, {name: "CartId", unique: true});

db.orders.insertMany([
    {
        "OrderId": 100,
        "CartId": 5,
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
        "CartId": 6,
        "Lines": [
            {
                "Position": 1,
                "ProductId": 500
            }
        ]
    }
]);
