db.auth('root', 'password')

db = db.getSiblingDB('Lackbot')

db.createUser({
    user: 'lackbot',
    pwd: 'lackbot',
    roles: [
        {
            role: 'readWrite',
            db: 'Lackbot',
        },
    ],
});
