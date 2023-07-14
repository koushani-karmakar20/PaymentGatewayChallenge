import * as React from 'react';
import Box from '@mui/material/Box';
import Card from '@mui/material/Card';
import CardActions from '@mui/material/CardActions';
import CardContent from '@mui/material/CardContent';
import Button from '@mui/material/Button';
import Typography from '@mui/material/Typography';

const bull = (
  <Box
    component="span"
    sx={{ display: 'inline-block', mx: '2px', transform: 'scale(0.8)' }}
  >
    •
  </Box>
);

export default function BasicCard(props) {
  return (
    <Card sx={{ minWidth: 275 }}>
      <CardContent>
        <Typography sx={{ fontSize: 14 }} color="text.secondary" gutterBottom>
          Payment Id: {props.prop.id}
        </Typography>
        <Typography variant="h6" component="div">
          Card Number: {props.prop.customer_card_number}
        </Typography>
        <Typography variant="h6" component="div">
          Timestamp: {props.prop.timestamp}
        </Typography>
        <Typography sx={{ mb: 1.5 }} color="text.secondary">
         {props.prop.description}
        </Typography>
      </CardContent>
      
    </Card>
  );
}