import React, { useState } from "react";
import DatePicker from "react-datepicker";
import "react-datepicker/dist/react-datepicker.css";
import styles from "./styles.module.css";
import "./style.css";

const MyDatePicker = ({ date, setDate, label, minDate }) => {
  return (
    <div className={styles.myInput}>
      <p className={styles.label}>{label}:</p>
      <DatePicker
        className="dddd"
        selected={date}
        onChange={(date) => setDate(date)}
        minDate={minDate}
      ></DatePicker>
    </div>
  );
};

export default MyDatePicker;
