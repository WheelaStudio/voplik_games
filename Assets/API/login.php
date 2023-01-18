<?php

include 'database.php';
$name = $_POST['username'];
$pass = $_POST['password'];



$sql = $users_db->query("SELECT * FROM users WHERE name = '".$name."'");
if ($sql->rowCount() == 0) {
    echo "User doesn't exist";
    die;
}
while($row = $sql->fetch()){
    if(password_verify($pass, $row['password'])){
        $arr = array('UserName' => $row['name'], 'Coins' => $row['coins'], 'Id' => $row['id']);
        echo json_encode($arr);
        die;
    }
    else{
        echo "Wrong password";
        die;
    }
}



?>