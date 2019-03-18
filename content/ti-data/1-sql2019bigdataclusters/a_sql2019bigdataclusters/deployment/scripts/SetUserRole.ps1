function Start-ImmersionPostDeployScript {
    param(
        $Credentials,
        $TenantId,
        $Region,
        $UserEmail,
        $UserPassword,
        $resourceGroupName,
        $StorageAccountName
    )


	function AssignUserRole ($RoleDefinitionName) {
        Write-Verbose "Assigning role '$RoleDefinitionName' to $UserEmail"
        if(!(Get-AzureRmRoleAssignment -SignInName $UserEmail -ResourceGroupName $ResourceGroupName -RoleDefinitionName $RoleDefinitionName)) {
            New-AzureRmRoleAssignment -SignInName $UserEmail -ResourceGroupName $ResourceGroupName -RoleDefinitionName $RoleDefinitionName | Out-Null
        }
        else {
            Write-Warning "Role '$RoleDefinitionName' already assigned!"
        }
    }

    #Assign roles required for the current story
    AssignUserRole -RoleDefinitionName 'Contributor'

    $existDefs = Get-AzureRmPolicyDefinition
    $existDef =  $existDefs | Where { $_.Name -eq "RestrictToWindowsServer" }
    if ($existDef) 
    {
       $ResourceGroup = Get-AzureRmResourceGroup -Name $resourceGroupName
       #$AssignPolicy1 = New-AzureRmPolicyAssignment -Name RestrictToWindowsServer -DisplayName RestrictToWindowsServer -PolicyDefinition $existDef -Scope $ResourceGroup.ResourceId -ApiVersion "2018-05-01"
    }
}