<?php

include 'database.php';

$coins = $_POST['coins'];
$user_id = $_POST['id'];

$sql = $users_db->query("UPDATE users SET coins = '" . $coins . "' WHERE id = '" . $user_id . "' ");
$sql->fetch();
echo ("Successfully");

?>