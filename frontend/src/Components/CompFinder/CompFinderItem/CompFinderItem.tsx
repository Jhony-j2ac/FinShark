import React, { useState } from 'react'
import { CompanyCompData } from '../../../company';
import { Link } from 'react-router';
import { v4 } from 'uuid';

type Props = {
    ticker: string;
}

const CompFinderItem = ({ticker}: Props) => {
  return <Link key={v4()}
    to={`/company/${ticker}/company-profile`}
    type='button' className='inline-flex items-center px-4 rounded-l-lg'
  >
    ticker
  </Link>
}

export default CompFinderItem