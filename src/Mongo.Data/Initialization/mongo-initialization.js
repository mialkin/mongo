db = db.getSiblingDB("example");

db.createCollection("orders");

db.configuration.insert([
    {
        "OrderId": 100,
        "Lines": [
            {
                "Position": 1,
                "ProductId": 4444,
                "Price": 9.99
            },
            {
                "Position": 2,
                "ProductId": 4455,
                "Price": 24.99
            }
        ]
    },
    {
        "OrderId": 101,
        "Lines": [
            {
                "Position": 1,
                "ProductId": 500,
                "Price": 30
            }
        ]
    }
]);
