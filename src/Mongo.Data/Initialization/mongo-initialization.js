db = db.getSiblingDB("example");

db.createCollection("orderIdSequence");
db.orderIdSequence.insert({
    "Value": 2
});

db.createCollection("orders");
db.orders.createIndex({"OrderId": 1}, {name: "OrderId", unique: true});
db.orders.createIndex({"CartId": 1}, {name: "CartId", unique: true});

db.orders.insertMany([
    {
        "OrderId": 1,
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
        "OrderId": 2,
        "CartId": 6,
        "Lines": [
            {
                "Position": 1,
                "ProductId": 500
            }
        ]
    }
]);
