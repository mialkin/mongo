db = db.getSiblingDB("example");

db.createCollection("orders");

db.orders.insertMany([
    {
        "orderId": 100,
        "lines": [
            {
                "position": 1,
                "productId": 4444
            },
            {
                "position": 2,
                "productId": 4455
            }
        ]
    },
    {
        "orderId": 101,
        "lines": [
            {
                "position": 1,
                "productId": 500
            }
        ]
    }
]);
