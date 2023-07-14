import * as React from 'react';
import Box from '@mui/material/Box';
import Tabs from '@mui/material/Tabs';
import Tab from '@mui/material/Tab';
import MerchantPayments from './MerchantPayments';
import MerchantDetails from './MerchantDetails';

export default function CenteredTabs() {
  const [value, setValue] = React.useState(0);
  const [isShown, setIsShown] = React.useState(false);

  const handleChange = (event, newValue) => {
    setValue(newValue);
    setIsShown(!isShown);
  };

const handleClick = event => {
    // 👇️ toggle shown state
    setIsShown(isShown => !isShown);

    // 👇️ or simply set it to true
    // setIsShown(true);
  };

  // React.useEffect()
  return (
    <div>
      <Box sx={{ width: '100%', bgcolor: 'background.paper' }}>
        <Tabs value={value} onChange={handleChange} centered>
          <Tab label="Merchant Details" />
          <Tab label="Merchant Payments" />
        </Tabs>
      </Box>



      <div id="Container">
        {!isShown && <MerchantDetails />}
        {isShown && <MerchantPayments />}
      </div>
    </div>
  );
}