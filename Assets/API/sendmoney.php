<?php

include 'database.php';

$money = $_POST['coins'];

$r_name = $_POST['r_name'];
$sender_name = $_POST['s_name'];


$sql = $users_db->query("UPDATE users SET coins = coins + '" . $money . "' WHERE name = '" . $r_name . "' ");
$sql->fetch();

if($sql->rowCount() == 0){
    sendMsg("Warning", "User doesn't exist");
    die;
}

$sql = $users_db->query("UPDATE users SET coins = coins - '" . $money . "' WHERE name = '" . $sender_name . "' ");
$sql->fetch();


$sql = $users_db->prepare("INSERT INTO transactions VALUES (:from, :to, :sum, null, null)");
$sql->bindParam(':from', $sender_name);
$sql->bindParam(':to', $r_name);
$sql->bindParam(':sum', $money);

$sql->execute();

sendMsg("Success", "You sent $money to $r_name");

function sendMsg($type, $message){
    $arr = array('Type' => $type, 'Message' => $message);
    echo json_encode($arr);
}

?>