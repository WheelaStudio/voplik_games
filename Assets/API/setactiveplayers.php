<?php

include 'database.php';

$count = $_POST['count'];

$sql = $users_db->query(" UPDATE player_count SET p_count = '" . $count . "' WHERE id = '1' ");
$sql->fetch();
echo "Success";

?>