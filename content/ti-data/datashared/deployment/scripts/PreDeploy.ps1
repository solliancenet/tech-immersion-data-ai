
# NOTE : This pre deploy script will create a copy of vhd's and place them in the root of a container.
# This is to resolve a issue with attaching to a vhd's that are not already in the container root. 

function Start-ImmersionPreDeployScript {
  param(
    $Credentials,
    $TenantId,
    $Region,
    $UserEmail,
    $UserPassword,
    $ResourceGroupName,
    $StorageAccountName,
    $ProvisioningId
  )
  
  # set values
  $vhdBlobName = 'labvmv3.vhd'
  $vhdBlobSource = 'ti-data/20190326/labvmv3.vhd'
  $vhdSourceContainer = 'assets'
      
  #replacing standard storage with premium
  $StorageAccountName = $StorageAccountName -replace "s1$", "p1"
  
  $StorageAccountKey = Get-AzureRmStorageAccountKey -ResourceGroupName 'SharedResources' -Name $StorageAccountName
  
  if (-not $StorageAccountKey -or $StorageAccountKey.Length -eq 0) {
    throw "Could not retrieve storage account key $($StorageAccountName)"
  }
     
  # copy vhd
  $key = (Get-AzureRmStorageAccountKey -ResourceGroupName 'SharedResources' -Name $StorageAccountName)[0].Value
  $ctx = New-AzureStorageContext -StorageAccountName $StorageAccountName -StorageAccountKey $key
   
  #check if container exists, if not - create it
  $ProvisioningContainer = Get-AzureStorageContainer -Name $ProvisioningId* -Context $ctx
  
  if ($ProvisioningContainer -eq $null) {
    $ProvisioningContainer = New-AzureStorageContainer -Name $ProvisioningId -Context $ctx
  }
  
  $copy = Start-AzureStorageBlobCopy -Context $ctx `
    -SrcContainer $vhdSourceContainer `
    -SrcBlob $vhdBlobSource `
    -DestContext $ctx `
    -DestContainer $ProvisioningId `
    -DestBlob $vhdBlobName `
    -ErrorAction Stop
  
  $copy | Get-AzureStorageBlobCopyState -WaitforComplete    
}