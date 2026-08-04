import React, { useEffect, useState } from "react";
import { Title } from "../../../common.styled";

import CheckBox from "../../../Checkbox/CheckBox";
import styles from "./styles.module.css";
// import { distributorData } from "../../../../assets/data";
import { Link } from "react-router-dom";
import { format } from 'date-fns';

const Table = ({tabledata}) => {

  const heading = [
    "REG  DATE",
    "MOBILE NO.",
    "STATUS",
    "REGION",
    "BALANCE",
    "DISTRIBUTOR CODE",
    "History",
  ];

  return (
    <div className={styles.myTable}>
      <div className={` ${styles.headingContainer}`}>
        {heading.map((el, i) => (
          <Title key={i} fontFamily=" 'Quicksand', sans-serif" padding="15px 0">
            {el}
          </Title>
        ))}
      </div>
      <div className={styles.table}>
        {tabledata.length>0 ? tabledata.map((el, i) => (
          <div key={i} className={styles.tableContainer}>
            <Title
              padding="12px 0"
              fontFamily=" 'Quicksand', sans-serif"
              className={styles.title}
              mobileFontSize="15px"
            >
              {format(new Date(el.reg_date), 'dd/MM/yyyy')}
            </Title>
            <Title
              padding="12px 0"
              fontFamily=" 'Quicksand', sans-serif"
              className={styles.title}
              mobileFontSize="15px"
            >
              {el.mobile_number}
            </Title>
            <Title
              padding="12px 0"
              fontFamily=" 'Quicksand', sans-serif"
              className={styles.title}
              mobileFontSize="15px"
            >
              {el.status}
            </Title>{" "}
            <Title
              padding="12px 0"
              fontFamily=" 'Quicksand', sans-serif"
              className={styles.title}
              mobileFontSize="15px"
            >
              {el.region}
            </Title>
            <Title
              padding="12px 0"
              fontFamily=" 'Quicksand', sans-serif"
              className={styles.title}
              mobileFontSize="15px"
            >
              {el.balance}
            </Title>{" "}
            <Title
              center
              padding="12px 0"
              fontFamily=" 'Quicksand', sans-serif"
              className={styles.title}
              mobileFontSize="15px"
            >
              {el.distributor_code}
            </Title>{" "}
            <Link to={`/distributorDetails/${el.id}`} className={styles.viewButton}>
              VIEW NOW
            </Link>
          </div>
        )) : <p className={`${styles.title}`} style={{'textAlign': 'center'}}>No Record found</p>}
      </div>
    </div>
  );
};

export default Table;
