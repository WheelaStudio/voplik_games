<?php

include 'database.php';

$step = $_POST['max'];
$min = $_POST['min'];
$player_name = $_POST['name'];

$sql = $users_db->query("SELECT * FROM transactions WHERE (from_name='" . $player_name . "' || to_name='" . $player_name . "') ORDER BY id DESC LIMIT $min, $step ");
$result = $sql->fetchAll();

$arr1 = array();

foreach ($result as $row){

    array_push($arr1, array('From' => $row['from_name'], 'To' => $row['to_name'], 'Sum' => $row['coins'], 'Id' => $row['id'], 'Date' => $row['transaction_date']));

}

echo json_encode($arr1);




?>