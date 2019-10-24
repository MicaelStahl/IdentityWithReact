import React, { Component } from "react";
import Title from "../../UI/Title";

class Edit extends Component {
  state = {
    firstName: null,
    lastName: null,
    age: null,
    email: null,
    phoneNumber: null,
    city: null,
    postalCode: null
  };
  render() {
    return (
      <React.Fragment>
        <Title
          Title={`Editing ${this.props.person.firstName +
            this.props.person.lastName}`}
        />
      </React.Fragment>
    );
  }
}

export default Edit;
