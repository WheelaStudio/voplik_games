<?php

include 'database.php';

$sql = $users_db->query("SELECT p_count FROM player_count WHERE id = 1");
$result = $sql->fetch();

echo $result['p_count'];

?>