import React from 'react'
import Table from '../../Components/Table/Table'
import RadioList from '../../Components/RadioList/RadioList'
import { TestDataCompany, testIncomeStatementData } from '../../Components/Table/TestData';

type Props = {}

const tableConfig = [
  {
    label: "Market Cap",
    render: (company: any) => company.marketCapTTM,
    subTitle: "Total value of all a company's shares of stock",
  },
];

const DesignPage = (props: Props) => {
  return (
    <>
        <h1>Finshark design page</h1>
        <h2>This is where we well house various design aspects of the app</h2>
        <RadioList data={testIncomeStatementData} config={tableConfig} />
        <Table configs={tableConfig} data={testIncomeStatementData} />
    </>
  )
}

export default DesignPage