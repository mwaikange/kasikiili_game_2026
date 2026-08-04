import React, { useState } from "react";
import styles from "./CheckBox.module.css";

const CheckBox = ({ label, disabled, data, index, onClick }) => {
  const [checked, setChecked] = useState(false);

  return (
    <label className={styles.container}>
      <input
        type="checkbox"
        disabled={disabled}
        value={data.checked}
        checked={data.checked}
        id={index}
        onChange={() => data.checked = !data.checked}
        className={styles.myCheckBox}
        onClick={() => onClick()}
      />
      <span className={styles.checkmark}></span> {label}
    </label>
  );
};

export default CheckBox;
