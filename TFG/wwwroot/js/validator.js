$(document).ready(function () {
    $('#crearPlanificacionAsignaturaForm').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            HorasSemanalesPlaAsi: {
                validators: {
                    notEmpty: {
                        message: "El campo horas semanales es obligatorio"
                    },
                    between: {
                        min: 1,
                        max: 30,
                        message: 'Las horas semanales deben estar comprendidas entre 1 y 30'
                    }
                }
            }
        }
    });
});


$(document).ready(function () {
    $('#crearGrupoForm').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            Descripcion: {
                validators: {
                    notEmpty: {
                        message: "El campo descripción es obligatorio"
                    },
                    stringLength: {
                        max: 50,
                        message: 'No puede superar los 50 caracteres'

                    }
                }
            }
        }
    });
});



$(document).ready(function () {
    $('#editarPlanificacionReduccionForm').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            DescripcionPlaRed: {
                validators: {
                    notEmpty: {
                        message: "El campo descripción es obligatorio"
                    },
                    stringLength: {
                        max: 50,
                        message: 'No puede superar los 50 caracteres'

                    }
                }
            },           
            HorasSemanalesPlaRed: {
                validators: {
                    notEmpty: {
                        message: "El campo horas semanales es obligatorio"
                    },
                    between: {
                        min: 1,
                        max: 30,
                        message: 'Las horas semanales deben estar comprendidas entre 1 y 30'
                    }
                }
            }
        }
    });
});



$(document).ready(function () {
    $('#crearPlanificacionReduccionForm').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            Descripcion: {
                validators: {
                    notEmpty: {
                        message: "El campo descripción es obligatorio"
                    },
                    stringLength: {
                        max: 50,
                        message: 'No puede superar los 50 caracteres'

                    }
                }
            },
            HorasSemanales: {
                validators: {
                    notEmpty: {
                        message: "El campo horas semanales es obligatorio"
                    },
                    between: {
                        min: 1,
                        max: 30,
                        message: 'Las horas semanales deben estar comprendidas entre 1 y 30'
                    }
                }
            }
        }
    });
});


/*
Validación de los campos de la ventana modal para crear Nueva planificacion
*/
$(document).ready(function () {
    $('#crearPlanificacionForm').bootstrapValidator({
        feedbackIcons: {
            valid: 'glyphicon glyphicon-ok',
            invalid: 'glyphicon glyphicon-remove',
            validating: 'glyphicon glyphicon-refresh'
        },
        fields: {
            Descripcion: {
                validators: {
                    notEmpty: {
                        message: "El campo descripción es obligatorio"
                    },
                    stringLength: {
                        max: 50,
                        message: 'No puede superar los 50 caracteres'

                    }
                }
            },
            HorasSemanales: {
                validators: {
                    notEmpty: {
                        message: "El campo horas semanales es obligatorio"
                    },
                    between: {
                        min: 1,
                        max: 30,
                        message: 'Las horas semanales deben estar comprendidas entre 1 y 30'
                    }
                }
            }
        }
    });


});



