import * as React from 'react';
import { styled } from '@mui/material/styles';
import Box from '@mui/material/Box';
import Paper from '@mui/material/Paper';
import Grid from '@mui/material/Unstable_Grid2';
import Button from '@mui/material/Button';
import TextField from '@mui/material/TextField';
import InputBase from '@mui/material/InputBase';
import Divider from '@mui/material/Divider';
import IconButton from '@mui/material/IconButton';
import MenuIcon from '@mui/icons-material/Menu';
import SearchIcon from '@mui/icons-material/Search';
import DirectionsIcon from '@mui/icons-material/Directions';
import Switch from '@mui/material/Switch';
import Typography from '@mui/material/Typography';
import axios from 'axios';

const Item = styled(Paper)(({ theme }) => ({
  backgroundColor: theme.palette.mode === 'dark' ? '#1A2027' : '#fff',
  ...theme.typography.body2,
  padding: theme.spacing(1),
  textAlign: 'center',
  color: theme.palette.text.secondary,
}));

export default function BasicGrid() {

    const [apiKey, setApiKey] = React.useState("");
    const [isActive, setIsActive] = React.useState(false);
    const [merchantId, setMerchantId] = React.useState("");

    // const label = { inputProps: { 'aria-label': 'Switch demo' } };
    function updateMerchant(event) {
        setMerchantId(event.target.value);
    }

    function fetchMerchant() {
        axios.get('https://paymentgateway-koushani.azurewebsites.net/api/PaymentGateway/GetMerchantWithId?id='+merchantId)
  .then(function (response) {
    // handle success
    console.log(response);
    setApiKey(response.data.apI_key);
    setIsActive(response.data.is_active);
  })
  .catch(function (error) {
    // handle error
    console.log(error);
  })
  .finally(function () {
    // always executed
  });
    }

  return (
    <Box sx={{ flexGrow: 1 }}>
        <br /><br /><br /><br /><br />
      <Grid container spacing={2} >
      <Grid xs={4}>
        </Grid>
        <Grid xs={8}>
            <Paper 
                component="form"
                sx={{ p: '2px 4px', display: 'flex', alignItems: 'center', width: 400 }}
                >
                <IconButton sx={{ p: '10px' }} aria-label="menu">
                    {/* <MenuIcon /> */}
                </IconButton>
                <InputBase
                    sx={{ ml: 1, flex: 1 }}
                    placeholder="Search Merchant Id"
                    inputProps={{ 'aria-label': 'search google maps' }}
                    onChange={updateMerchant}
                />
                <IconButton type="button" sx={{ p: '10px' }} aria-label="search">
                    <SearchIcon onClick={fetchMerchant} />
                </IconButton>
            </Paper>
        </Grid>
        <Grid xs={8}>
            <Item>
                <Typography variant="h6" gutterBottom>
                API Key: {apiKey}
                </Typography>
            </Item>
        </Grid>
        <Grid xs={4}>
          <Item>
              Active: <Switch checked={isActive} defaultChecked />
          </Item>
        </Grid>
      </Grid>
    </Box>
  );
}
