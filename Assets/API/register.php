<?php

include 'database.php';
$name = $_POST['username'];
$user_password = $_POST['password'];

$sql = $users_db->prepare('INSERT INTO users VALUES (:name, :pass, 0, null)');

$password_hash = password_hash($user_password, PASSWORD_DEFAULT);

try{
    $sql->bindParam(':name', $name);
    $sql->bindParam(':pass', $password_hash);
    $sql2 = $users_db->query("SELECT name FROM users WHERE name = '" . $name . "'");
    if($sql2->rowCount() > 0) {
        echo("Username already exists");
        die();
    }
    else{
        echo "New user created";
        $sql->execute();
    }
}
catch(Exception $e){
    echo "Error: " . $e->getMessage();
}


?>