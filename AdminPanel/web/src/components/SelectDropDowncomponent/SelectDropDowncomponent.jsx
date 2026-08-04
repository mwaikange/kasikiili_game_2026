import React, { useState } from "react";
import { AiFillCaretUp, AiFillCaretDown } from "react-icons/ai";

import styles from "./styles.module.css";

const SelectDropDowncomponent = ({ title, top, items, value, setValue, onChange }) => {
  const [dropwDown, setDropDown] = useState(false);

  const changevalue = (e) => {
    console.log("e------", e.target.value);
    setValue(e.target.value)
  }

  return (
    <div className={styles.wrapper}>
      <p className={styles.title}>{title}</p>
      <select className={styles.dropDowns_select} value={value} onChange={(e) => onChange(e)}>
        <option value=''>{title}</option>
        {items.map((el, i) => (
          <option
            className={styles.items}
            key={i}
            value={el}
          >
            {el}{" "}
          </option>
        ))}
      </select>
    </div>
  );
};

export default SelectDropDowncomponent;